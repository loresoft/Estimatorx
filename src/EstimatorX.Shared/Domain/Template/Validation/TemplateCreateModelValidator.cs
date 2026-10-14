using EstimatorX.Domain.Models;

using FluentValidation;

namespace EstimatorX.Domain.Validation;

[RegisterSingleton<IValidator<TemplateCreateModel>>]
public partial class TemplateCreateModelValidator
    : AbstractValidator<TemplateCreateModel>
{
    public TemplateCreateModelValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Name).MaximumLength(255);
        RuleFor(p => p.Description).MaximumLength(2000);
        RuleFor(p => p.Settings).NotEmpty();
    }
}
