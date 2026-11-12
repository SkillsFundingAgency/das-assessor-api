using AutoFixture.NUnit4;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.Assessor.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.Assessor.Domain.Interfaces;
using System;

namespace SFA.DAS.Assessor.Application.UnitTests.Commands.CreateOrUpdateUser
{
    public class WhenValidatingCreateOrUpdateUserCommand
    {
        [Test, AutoData]
        public void Then_Command_Is_Valid_When_All_Fields_Are_Correct(
            [Frozen] Mock<IDateTimeProvider> mockDateTimeProvider)
        {
            // Arrange
            var now = new DateTime(2025, 11, 10, 0, 0, 0, DateTimeKind.Unspecified);
            mockDateTimeProvider.Setup(x => x.Now).Returns(now);

            var command = new CreateOrUpdateUserCommand
            {
                DfeSignInUserId = "dfe-123",
                EmailAddress = "user@example.com"
            };

            var validator = new CreateOrUpdateUserCommandValidator(mockDateTimeProvider.Object);

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test, AutoData]
        public void Then_Error_If_DfeSignInUserId_Is_Empty([Frozen] Mock<IDateTimeProvider> mockDateTimeProvider)
        {
            var command = new CreateOrUpdateUserCommand
            {
                DfeSignInUserId = "",
                EmailAddress = "user@example.com"
            };

            var validator = new CreateOrUpdateUserCommandValidator(mockDateTimeProvider.Object);
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.DfeSignInUserId)
                .WithErrorMessage("DfeSignInUserId must not be empty");
        }

        [Test, AutoData]
        public void Then_Error_If_EmailAddress_Is_Empty([Frozen] Mock<IDateTimeProvider> mockDateTimeProvider)
        {
            var command = new CreateOrUpdateUserCommand
            {
                DfeSignInUserId = "dfe-123",
                EmailAddress = ""
            };

            var validator = new CreateOrUpdateUserCommandValidator(mockDateTimeProvider.Object);
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.EmailAddress)
                .WithErrorMessage("EmailAddress must not be empty");
        }
    }
}