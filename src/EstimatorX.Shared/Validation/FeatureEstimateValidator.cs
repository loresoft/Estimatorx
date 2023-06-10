
using EstimatorX.Shared.Models;

using FluentValidation;

namespace EstimatorX.Shared.Validation;

public class FeatureEstimateValidator : AbstractValidator<FeatureEstimate>
{
    public FeatureEstimateValidator()
    {
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Clarity).NotEmpty().When(p => p.Estimate != null).WithMessage("Clarity required when Estimate is provided.");
        RuleFor(p => p.Confidence).NotEmpty().When(p => p.Estimate != null).WithMessage("Confidence required when Estimate is provided.");
    }
}
