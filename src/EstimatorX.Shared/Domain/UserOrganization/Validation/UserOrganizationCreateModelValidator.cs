using EstimatorX.Domain.Models;

using FluentValidation;

namespace EstimatorX.Domain.Validation;

[RegisterSingleton<IValidator<UserOrganizationCreateModel>>]
public partial class UserOrganizationCreateModelValidator
    : AbstractValidator<UserOrganizationCreateModel>
{
    public UserOrganizationCreateModelValidator()
    {
    }
}
