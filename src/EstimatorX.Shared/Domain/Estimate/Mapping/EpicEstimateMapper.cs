#pragma warning disable IDE0130 // Namespace does not match folder structure

using EstimatorX.Domain.Models;

namespace EstimatorX.Domain.Mapping;

[RegisterSingleton]
internal sealed class EpicEstimateMapper
    : EstimateMapper<EpicEstimate>
{
    public override void Map(EpicEstimate source, EpicEstimate destination)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.Name = source.Name;
        destination.Description = source.Description;
        destination.Assumptions = source.Assumptions;

        destination.EstimatedTotal = source.EstimatedTotal;
        destination.WeightedTotal = source.WeightedTotal;
        destination.EstimatedWeeks = source.EstimatedWeeks;
        destination.WeightedWeeks = source.WeightedWeeks;
        destination.EstimatedCost = source.EstimatedCost;
        destination.WeightedCost = source.WeightedCost;

        destination.Features = source.Features.ConvertAll(EstimateMapper.CloneFeature);
    }
}
