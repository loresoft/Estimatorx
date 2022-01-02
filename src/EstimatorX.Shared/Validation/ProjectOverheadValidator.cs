
using EstimatorX.Shared.Models;

using FluentValidation;

namespace EstimatorX.Shared.Validation;

public class ProjectOverheadValidator : AbstractValidator<ProjectOverhead>
{
    public ProjectOverheadValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Multiplier).GreaterThanOrEqualTo(1);
    }
}
