using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

public class EpicEstimate : IHaveIdentifier, IHaveName
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public string Name { get; set; }

    public string Description { get; set; }

    public string Assumptions { get; set; }

    public List<FeatureEstimate> Features { get; set; } = new();

    // computed
    public int? EstimatedTotal { get; set; }

    public int? WeightedTotal { get; set; }

    public double? EstimatedCost { get; set; }

    public double? WeightedCost { get; set; }


    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Id);
        hash.Add(Name);
        hash.Add(Description);
        hash.Add(EstimatedTotal);
        hash.Add(WeightedTotal);
        hash.Add(EstimatedCost);
        hash.Add(WeightedCost);

        foreach (var feature in Features)
            hash.Add(feature.GetHashCode());

        return hash.ToHashCode();
    }

}
