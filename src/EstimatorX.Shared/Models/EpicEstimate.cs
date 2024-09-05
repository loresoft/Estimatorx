using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

[Equatable]
public partial class EpicEstimate : IHaveIdentifier, IHaveName
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public string Name { get; set; }

    public string Description { get; set; }

    public string Assumptions { get; set; }

    [SequenceEquality]
    public List<FeatureEstimate> Features { get; set; } = new();

    // computed
    public int? EstimatedTotal { get; set; }

    public int? WeightedTotal { get; set; }

    public double? EstimatedCost { get; set; }

    public double? WeightedCost { get; set; }
}
