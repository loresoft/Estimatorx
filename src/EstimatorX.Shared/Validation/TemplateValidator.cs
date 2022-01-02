
using EstimatorX.Shared.Models;

using FluentValidation;

namespace EstimatorX.Shared.Validation;

public class TemplateValidator : AbstractValidator<Template>
{
    public TemplateValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
    }
}
