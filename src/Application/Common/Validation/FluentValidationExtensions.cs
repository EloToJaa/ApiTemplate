using FluentValidation;

namespace Application.Common.Validation;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> BeValidGuid<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder.Must((rootObject, propertyValue, context) =>
        {
            return propertyValue is null || Guid.TryParse(propertyValue?.ToString(), out _);
        })
        .WithMessage("Invalid GUID format.");
    }
}
