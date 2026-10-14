using EstimatorX.Domain.Models;

using FluentValidation;

namespace EstimatorX.Domain.Validation;

[RegisterSingleton<IValidator<UserCreateModel>>]
public partial class UserCreateModelValidator
    : AbstractValidator<UserCreateModel>
{
    public UserCreateModelValidator()
    {
        RuleFor(p => p.Email).MaximumLength(256);
        RuleFor(p => p.Name).MaximumLength(256);
    }
}
