
using EstimatorX.Shared.Models;

using FluentValidation;

namespace EstimatorX.Shared.Validation;

public class EstimateMultiplierValidator : AbstractValidator<EstimateMultiplier>
{
    public EstimateMultiplierValidator()
    {
        RuleFor(p => p.Value).GreaterThanOrEqualTo(1);
    }
}
