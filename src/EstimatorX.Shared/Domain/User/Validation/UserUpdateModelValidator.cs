using EstimatorX.Domain.Models;

using FluentValidation;

namespace EstimatorX.Domain.Validation;

[RegisterSingleton<IValidator<UserUpdateModel>>]
public partial class UserUpdateModelValidator
    : AbstractValidator<UserUpdateModel>
{
    public UserUpdateModelValidator()
    {
        RuleFor(p => p.Email).MaximumLength(256);
        RuleFor(p => p.Name).MaximumLength(256);
    }
}
