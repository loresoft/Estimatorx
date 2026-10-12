using EstimatorX.Domain.Models;

using FluentValidation;

namespace EstimatorX.Domain.Validation;

[RegisterSingleton<IValidator<UserLoginUpdateModel>>]
public partial class UserLoginUpdateModelValidator
    : AbstractValidator<UserLoginUpdateModel>
{
    public UserLoginUpdateModelValidator()
    {
        RuleFor(p => p.UserId).NotEmpty();
        RuleFor(p => p.IpAddress).MaximumLength(50);
    }
}
