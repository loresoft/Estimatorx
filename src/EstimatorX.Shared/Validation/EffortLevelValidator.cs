
using EstimatorX.Shared.Models;

using FluentValidation;

namespace EstimatorX.Shared.Validation;

public class EffortLevelValidator : AbstractValidator<EffortLevel>
{
    public EffortLevelValidator()
    {
        RuleFor(p => p.Level).NotEmpty();
        RuleFor(p => p.Effort).NotNull();
    }
}
