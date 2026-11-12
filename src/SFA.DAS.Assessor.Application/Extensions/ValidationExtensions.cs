using FluentValidation;

namespace SFA.DAS.Assessor.Application.Extensions
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string?> MustNotContainHtmlTags<T>(
        this IRuleBuilder<T, string?> ruleBuilder, string propertyName)
        {
            return ruleBuilder
                .Matches(@"^[^<>]*$")
                .WithName(propertyName)
                .WithMessage("{PropertyName} contains invalid characters.");
        }
    }
}
