using EstimatorX.Domain.Models;

using FluentValidation;

namespace EstimatorX.Domain.Validation;

[RegisterSingleton<IValidator<OrganizationUpdateModel>>]
public partial class OrganizationUpdateModelValidator
    : AbstractValidator<OrganizationUpdateModel>
{
    public OrganizationUpdateModelValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Name).MaximumLength(100);
        RuleFor(p => p.Description).MaximumLength(255);
    }
}
