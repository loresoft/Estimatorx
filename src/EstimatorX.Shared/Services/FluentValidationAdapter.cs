using Arbiter.CommandQuery.Definitions;
using Arbiter.CommandQuery.Models;

namespace EstimatorX.Services;

[RegisterSingleton(ServiceType = typeof(IValidator<>), ImplementationType = typeof(FluentValidationAdapter<>))]
internal class FluentValidationAdapter<T>(FluentValidation.IValidator<T>? validator = null) : IValidator<T>
{
    public async ValueTask<ValidationResult> Validate(T instance, CancellationToken cancellationToken = default)
    {
        if (validator is null)
            return ValidationResult.Success;

        var result = await validator.ValidateAsync(instance, cancellationToken);
        if (result.IsValid)
            return ValidationResult.Success;

        return new ValidationResult
        {
            Errors = result.Errors
                .GroupBy(failure => failure.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(failure => failure.ErrorMessage).ToArray(),
                    StringComparer.Ordinal)
        };
    }
}
