namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class FeatureEstimate : EstimateBase
{
    [SequenceEquality]
    public List<StoryEstimate> Stories { get; set; } = [];
}
