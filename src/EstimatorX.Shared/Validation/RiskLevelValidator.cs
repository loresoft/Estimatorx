
using EstimatorX.Shared.Models;

using FluentValidation;

namespace EstimatorX.Shared.Validation;

public class RiskLevelValidator : AbstractValidator<RiskLevel>
{
    public RiskLevelValidator()
    {
        RuleFor(p => p.Risk).NotEmpty();
        RuleFor(p => p.Multiplier).GreaterThanOrEqualTo(1);
    }
}
