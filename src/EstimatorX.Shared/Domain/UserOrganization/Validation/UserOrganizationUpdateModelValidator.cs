using EstimatorX.Domain.Models;

using FluentValidation;

namespace EstimatorX.Domain.Validation;

[RegisterSingleton<IValidator<UserOrganizationUpdateModel>>]
public partial class UserOrganizationUpdateModelValidator
    : AbstractValidator<UserOrganizationUpdateModel>
{
    public UserOrganizationUpdateModelValidator()
    {
    }
}
