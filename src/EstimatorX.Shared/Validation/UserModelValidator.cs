
using EstimatorX.Shared.Models;

using FluentValidation;

namespace EstimatorX.Shared.Validation;

public class UserModelValidator : AbstractValidator<User>
{
    public UserModelValidator()
    {
        RuleFor(p => p.Email).NotEmpty();
    }
}
