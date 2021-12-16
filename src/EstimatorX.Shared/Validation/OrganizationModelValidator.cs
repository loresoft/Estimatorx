
using EstimatorX.Shared.Models;

using FluentValidation;

namespace EstimatorX.Shared.Validation;

public class OrganizationModelValidator : AbstractValidator<Organization>
{
    public OrganizationModelValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
    }
}
