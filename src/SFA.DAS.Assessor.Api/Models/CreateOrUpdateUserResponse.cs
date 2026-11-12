using System;
using SFA.DAS.Assessor.Application.Commands.CreateOrUpdateUser;

namespace SFA.DAS.Assessor.Api.Models
{
    public class CreateOrUpdateUserResponse
    {
        public Guid UserId { get; set; }

        public static implicit operator CreateOrUpdateUserResponse(CreateOrUpdateUserCommandResponse source)
        {
            if (source == null) return null!;

            return new CreateOrUpdateUserResponse
            {
                UserId = source.UserId
            };
        }
    }
}
