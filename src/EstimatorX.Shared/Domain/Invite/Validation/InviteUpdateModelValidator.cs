using EstimatorX.Domain.Models;

using FluentValidation;

namespace EstimatorX.Domain.Validation;

[RegisterSingleton<IValidator<InviteUpdateModel>>]
public partial class InviteUpdateModelValidator
    : AbstractValidator<InviteUpdateModel>
{
    public InviteUpdateModelValidator()
    {
        RuleFor(p => p.Email).NotEmpty();
        RuleFor(p => p.Email).MaximumLength(256);
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Name).MaximumLength(256);
        RuleFor(p => p.SecurityCode).MaximumLength(64);
    }
}
