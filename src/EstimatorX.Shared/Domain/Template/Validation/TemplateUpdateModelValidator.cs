using EstimatorX.Domain.Models;

using FluentValidation;

namespace EstimatorX.Domain.Validation;

[RegisterSingleton<IValidator<TemplateUpdateModel>>]
public partial class TemplateUpdateModelValidator
    : AbstractValidator<TemplateUpdateModel>
{
    public TemplateUpdateModelValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Name).MaximumLength(255);
        RuleFor(p => p.Description).MaximumLength(2000);
        RuleFor(p => p.Settings).NotEmpty();
    }
}
