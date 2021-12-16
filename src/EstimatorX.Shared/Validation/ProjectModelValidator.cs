
using EstimatorX.Shared.Models;

using FluentValidation;

namespace EstimatorX.Shared.Validation;

public class ProjectModelValidator : AbstractValidator<Project>
{
    public ProjectModelValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.OrganizationId).NotEmpty();
    }
}
