using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Assessor.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.Assessor.Domain.Entities;
using SFA.DAS.Assessor.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Assessor.Application.UnitTests.Commands.CreateOrUpdateUser
{
    public class WhenHandlingCreateOrUpdateUserCommand
    {
        private Mock<IUserEntityContext> _userEntityContextMock = null!;
        private Mock<IDateTimeProvider> _dateTimeProviderMock = null!;
        private CreateOrUpdateUserCommandHandler _sut = null!;

        private readonly DateTime _utcNow = DateTime.UtcNow;

        [SetUp]
        public void SetUp()
        {
            _userEntityContextMock = new Mock<IUserEntityContext>();
            _dateTimeProviderMock = new Mock<IDateTimeProvider>();

            _dateTimeProviderMock.SetupGet(x => x.UtcNow).Returns(_utcNow);

            _sut = new CreateOrUpdateUserCommandHandler(
                _dateTimeProviderMock.Object,
                _userEntityContextMock.Object);
        }

        [Test, MoqAutoData]
        public async Task And_UserDoesNotExist_Then_CreatesUserAndReturnsId(
            CreateOrUpdateUserCommand command)
        {
            var cancellationToken = CancellationToken.None;
            var persistedUserId = Guid.NewGuid();
            User? addedUser = null;

            _userEntityContextMock
                .Setup(x => x.Get(command.DfeSignInUserId))
                .ReturnsAsync((User?)null);

            _userEntityContextMock
                .Setup(x => x.TryCreateUserAsync(It.IsAny<User>(), cancellationToken))
                .Callback<User, CancellationToken>((user, _) =>
                {
                    user.Id = persistedUserId;
                    addedUser = user;
                })
                .ReturnsAsync(true);

            var result = await _sut.Handle(command, cancellationToken);

            addedUser.Should().NotBeNull();
            addedUser!.DfeSignInUserId.Should().Be(command.DfeSignInUserId);
            addedUser.EmailAddress.Should().Be(command.EmailAddress);
            addedUser.PhoneNumber.Should().Be(command.PhoneNumber);
            addedUser.CreatedAt.Should().Be(_utcNow);
            addedUser.LastLoginAt.Should().Be(_utcNow);
            result.UserId.Should().Be(persistedUserId);

            _userEntityContextMock.Verify(
                x => x.Get(command.DfeSignInUserId), Times.Once);
            _userEntityContextMock.Verify(
                x => x.TryCreateUserAsync(It.IsAny<User>(), cancellationToken), Times.Once);
            _userEntityContextMock.Verify(
                x => x.SaveChangesAsync(cancellationToken), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task And_UserExists_Then_UpdatesUserAndSavesChanges(
            CreateOrUpdateUserCommand command)
        {
            var cancellationToken = CancellationToken.None;
            var originalCreatedAt = _utcNow.AddMonths(-1);
            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                DfeSignInUserId = command.DfeSignInUserId,
                EmailAddress = "current@email.com",
                PhoneNumber = "0123456789",
                CreatedAt = originalCreatedAt
            };

            _userEntityContextMock
                .Setup(x => x.Get(command.DfeSignInUserId))
                .ReturnsAsync(existingUser);

            var result = await _sut.Handle(command, cancellationToken);

            existingUser.DfeSignInUserId.Should().Be(command.DfeSignInUserId);
            existingUser.EmailAddress.Should().Be(command.EmailAddress);
            existingUser.PhoneNumber.Should().Be(command.PhoneNumber);
            existingUser.LastLoginAt.Should().Be(_utcNow);
            existingUser.CreatedAt.Should().Be(originalCreatedAt);
            result.UserId.Should().Be(existingUser.Id);

            _userEntityContextMock.Verify(
                x => x.Get(command.DfeSignInUserId), Times.Once);
            _userEntityContextMock.Verify(
                x => x.TryCreateUserAsync(It.IsAny<User>(), cancellationToken), Times.Never);
            _userEntityContextMock.Verify(
                x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_UserIsCreatedConcurrently_Then_UpdatesAndReturnsExistingUser(
            CreateOrUpdateUserCommand command)
        {
            var cancellationToken = CancellationToken.None;
            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                DfeSignInUserId = command.DfeSignInUserId,
                EmailAddress = "initial@example.com",
                CreatedAt = _utcNow
            };

            _userEntityContextMock
                .SetupSequence(x => x.Get(command.DfeSignInUserId))
                .ReturnsAsync((User?)null)
                .ReturnsAsync(existingUser);

            _userEntityContextMock
                .Setup(x => x.TryCreateUserAsync(It.IsAny<User>(), cancellationToken))
                .ReturnsAsync(false);

            var result = await _sut.Handle(command, cancellationToken);

            result.UserId.Should().Be(existingUser.Id);
            existingUser.EmailAddress.Should().Be(command.EmailAddress);
            existingUser.PhoneNumber.Should().Be(command.PhoneNumber);
            existingUser.LastLoginAt.Should().Be(_utcNow);

            _userEntityContextMock.Verify(
                x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }
    }
}