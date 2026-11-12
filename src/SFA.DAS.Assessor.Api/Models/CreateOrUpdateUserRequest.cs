using System;
using SFA.DAS.Assessor.Application.Commands.CreateOrUpdateUser;

namespace SFA.DAS.Assessor.Api.Models
{
    public class CreateOrUpdateUserRequest
    {
        public required string DfeSignInUserId { get; set; }
        public required string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }

        public static implicit operator CreateOrUpdateUserCommand(CreateOrUpdateUserRequest source)
        {
            return new CreateOrUpdateUserCommand
            {
                DfeSignInUserId = source.DfeSignInUserId,
                EmailAddress = source.EmailAddress,
                PhoneNumber = source.PhoneNumber
            };
        }
    }
}
