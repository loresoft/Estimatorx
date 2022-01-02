
using EstimatorX.Shared.Models;

using FluentValidation;

namespace EstimatorX.Shared.Validation;

public class ProjectValidator : AbstractValidator<Project>
{
    public ProjectValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.OrganizationId).NotEmpty();

        RuleFor(p => p.Settings).SetValidator(new ProjectSettingsValidator());
    }
}
