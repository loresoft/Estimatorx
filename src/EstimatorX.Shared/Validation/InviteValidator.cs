
using EstimatorX.Shared.Models;

using FluentValidation;

namespace EstimatorX.Shared.Validation;

public class InviteValidator : AbstractValidator<Invite>
{
    public InviteValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Email).NotEmpty();
        RuleFor(p => p.Email).EmailAddress();
    }
}
