
using EstimatorX.Shared.Models;

using FluentValidation;

namespace EstimatorX.Shared.Validation
{
    public class ProjectModelValidator : AbstractValidator<ProjectModel>
    {
        public ProjectModelValidator()
        {
            RuleFor(p => p.Name).NotEmpty();
        }
    }
}
