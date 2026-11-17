using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Assessor.Domain.Entities;
using SFA.DAS.Assessor.Domain.Interfaces;

namespace SFA.DAS.Assessor.Application.Commands.CreateOrUpdateUser
{
    public class CreateOrUpdateUserCommandHandler : IRequestHandler<CreateOrUpdateUserCommand, CreateOrUpdateUserCommandResponse>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserEntityContext _userEntityContext;

        public CreateOrUpdateUserCommandHandler(
            IDateTimeProvider dateTimeProvider,
            IUserEntityContext userEntityContext)
        {
            _dateTimeProvider = dateTimeProvider;
            _userEntityContext = userEntityContext;
        }

        public async Task<CreateOrUpdateUserCommandResponse> Handle(
            CreateOrUpdateUserCommand command,
            CancellationToken cancellationToken)
        {
            var user = await _userEntityContext.Get(command.DfeSignInUserId);
            var lastLoginAt = _dateTimeProvider.UtcNow;

            if (user is null)
            {
                var newUser = new User
                {
                    DfeSignInUserId = command.DfeSignInUserId,
                    EmailAddress = command.EmailAddress,
                    PhoneNumber = command.PhoneNumber,
                    CreatedAt = _dateTimeProvider.UtcNow,
                    LastLoginAt = lastLoginAt
                };

                if (await _userEntityContext.TryCreateUserAsync(newUser, cancellationToken))
                {
                    return new CreateOrUpdateUserCommandResponse { UserId = newUser.Id };
                }

                user = await _userEntityContext.Get(command.DfeSignInUserId)
                    ?? throw new InvalidOperationException(
                        "The concurrently created Assessor user could not be found.");
            }

            user.EmailAddress = command.EmailAddress;
            user.PhoneNumber = command.PhoneNumber;
            user.LastLoginAt = lastLoginAt;

            await _userEntityContext.SaveChangesAsync(cancellationToken);

            return new CreateOrUpdateUserCommandResponse { UserId = user.Id };
        }
    }
}