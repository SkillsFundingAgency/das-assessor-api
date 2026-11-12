using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.Assessor.Application.Models;

namespace SFA.DAS.Assessor.Application.Commands.CreateOrUpdateUser
{
    public class CreateOrUpdateUserCommand : IRequest<CreateOrUpdateUserCommandResponse>
    {
        public required string DfeSignInUserId { get; set; }
        public required string EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
    }
}