using FluentValidation;
using SFA.DAS.Assessor.Domain.Interfaces;

namespace SFA.DAS.Assessor.Application.Commands.CreateOrUpdateUser
{
    public class CreateOrUpdateUserCommandValidator : AbstractValidator<CreateOrUpdateUserCommand>
    {
        public CreateOrUpdateUserCommandValidator(IDateTimeProvider dateTimeProvider)
        {
            RuleFor(x => x.DfeSignInUserId)
                .NotEmpty()
                .WithMessage("DfeSignInUserId must not be empty");

            RuleFor(x => x.EmailAddress)
                .NotEmpty()
                .WithMessage("EmailAddress must not be empty");
        }
    }
}
