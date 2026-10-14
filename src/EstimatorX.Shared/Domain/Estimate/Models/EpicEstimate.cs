using Arbiter.CommandQuery.Definitions;
using Arbiter.Services;

using EstimatorX.Domain.Definitions;

namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class EpicEstimate : IHaveIdentifier<long>, IHaveName
{
    public long Id { get; set; } = Snowflake.Default.NextId();

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Assumptions { get; set; }

    [SequenceEquality]
    public List<FeatureEstimate> Features { get; set; } = [];

    // computed
    public int? EstimatedTotal { get; set; }

    public int? WeightedTotal { get; set; }

    public double? EstimatedWeeks { get; set; }

    public double? WeightedWeeks { get; set; }


    public double? EstimatedCost { get; set; }

    public double? WeightedCost { get; set; }

    public override string ToString() => Name;
}
