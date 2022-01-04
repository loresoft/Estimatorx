
using EstimatorX.Shared.Models;

using FluentValidation;

namespace EstimatorX.Shared.Validation;

public class ProjectSettingsValidator : AbstractValidator<ProjectSettings>
{
    public ProjectSettingsValidator()
    {
        RuleForEach(p => p.EffortLevels).SetValidator(new EffortLevelValidator());
        RuleForEach(p => p.RiskLevels).SetValidator(new RiskLevelValidator());
        RuleForEach(p => p.Overhead).SetValidator(new ProjectOverheadValidator());
        RuleForEach(p => p.Multipliers).SetValidator(new EstimateMultiplierValidator());
    }
}
