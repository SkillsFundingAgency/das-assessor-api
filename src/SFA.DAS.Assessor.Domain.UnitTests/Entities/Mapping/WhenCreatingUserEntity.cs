using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Assessor.Domain.Entities;

namespace SFA.DAS.Assessor.Domain.UnitTests.Entities.Mapping
{
    public class WhenCreatingUserEntity
    {
        [Test]
        public void ThenTheFieldsAreCorrectlySet()
        {
            var id = Guid.NewGuid();
            var dfeSignInUserId = Guid.NewGuid().ToString();
            var email = "test@example.com";
            var phone = "0123456789";
            var lastLogin = DateTime.UtcNow;
            var isLocked = true;

            var user = new User
            {
                Id = id,
                DfeSignInUserId = dfeSignInUserId,
                EmailAddress = email,
                PhoneNumber = phone,
                LastLoginAt = lastLogin,
                IsLocked = isLocked
            };

            user.Id.Should().Be(id);
            user.EmailAddress.Should().Be(email);
            user.PhoneNumber.Should().Be(phone);
            user.LastLoginAt.Should().Be(lastLogin);
            user.IsLocked.Should().BeTrue();
        }
    }
}
