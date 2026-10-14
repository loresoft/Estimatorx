#pragma warning disable IDE0130 // Namespace does not match folder structure

using EstimatorX.Domain.Models;

namespace EstimatorX.Domain.Mapping;

[RegisterSingleton]
internal sealed class FeatureEstimateMapper
    : EstimateMapper<FeatureEstimate>
{
    public override void Map(FeatureEstimate source, FeatureEstimate destination)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        EstimateMapper.CopyEstimate(source, destination);
        destination.Stories = source.Stories.ConvertAll(EstimateMapper.CloneStory);
    }
}
