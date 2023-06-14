
using EstimatorX.Shared.Models;

using FluentValidation;

namespace EstimatorX.Shared.Validation;

public class TemplateValidator : AbstractValidator<Template>
{
    public TemplateValidator()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Name must not be empty.");
        RuleFor(p => p.OrganizationId).NotEmpty().WithMessage("Organization must not be empty.");

        RuleFor(p => p.Settings).SetValidator(new ProjectSettingsValidator());
        RuleForEach(p => p.Epics).SetValidator(new EpicEstimateValidator());
    }
}
