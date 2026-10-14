using EstimatorX.Domain.Models;

using FluentValidation;

namespace EstimatorX.Domain.Validation;

[RegisterSingleton<IValidator<UserProviderUpdateModel>>]
public partial class UserProviderUpdateModelValidator
    : AbstractValidator<UserProviderUpdateModel>
{
    public UserProviderUpdateModelValidator()
    {
        RuleFor(p => p.Provider).NotEmpty();
        RuleFor(p => p.Provider).MaximumLength(100);
        RuleFor(p => p.ProviderKey).NotEmpty();
        RuleFor(p => p.ProviderKey).MaximumLength(450);
    }
}
