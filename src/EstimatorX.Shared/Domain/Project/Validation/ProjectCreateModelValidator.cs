using EstimatorX.Domain.Models;

using FluentValidation;

namespace EstimatorX.Domain.Validation;

[RegisterSingleton<IValidator<ProjectCreateModel>>]
public partial class ProjectCreateModelValidator
    : AbstractValidator<ProjectCreateModel>
{
    public ProjectCreateModelValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Name).MaximumLength(255);
        RuleFor(p => p.Description).MaximumLength(2000);
        RuleFor(p => p.Settings).NotEmpty();
    }
}
