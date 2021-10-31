
using EstimatorX.Shared.Models;

using FluentValidation;

namespace EstimatorX.Shared.Validation;

public class TemplateModelValidator : AbstractValidator<TemplateModel>
{
    public TemplateModelValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
    }
}
