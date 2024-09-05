namespace EstimatorX.Shared.Models;

[Equatable]
public partial class FeatureEstimate : EstimateBase
{
    [SequenceEquality]
    public List<StoryEstimate> Stories { get; set; } = new();
}
