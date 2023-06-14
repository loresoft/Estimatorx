
using EstimatorX.Shared.Models;

using FluentValidation;

namespace EstimatorX.Shared.Validation;

public class EpicEstimateValidator : AbstractValidator<EpicEstimate>
{
    public EpicEstimateValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleForEach(p => p.Features).SetValidator(new FeatureEstimateValidator());
    }
}
