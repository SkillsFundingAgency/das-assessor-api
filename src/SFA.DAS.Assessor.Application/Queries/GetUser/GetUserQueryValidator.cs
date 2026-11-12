using FluentValidation;

namespace SFA.DAS.Assessor.Application.Queries.GetUser
{
    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator()
        {
            RuleFor(x => x.DfeSignInUserId).NotEmpty()
                .WithMessage("DfeSignInUserId must be provided.");
        }
    }
}
