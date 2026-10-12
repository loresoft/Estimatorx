#pragma warning disable IDE0130 // Namespace does not match folder structure

using EstimatorX.Domain.Models;

namespace EstimatorX.Domain.Mapping;

[RegisterSingleton]
internal sealed class StoryEstimateMapper
    : EstimateMapper<StoryEstimate>
{
    public override void Map(StoryEstimate source, StoryEstimate destination)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        EstimateMapper.CopyEstimate(source, destination);
    }
}
