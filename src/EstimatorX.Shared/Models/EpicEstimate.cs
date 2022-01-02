namespace EstimatorX.Shared.Models;

public class EpicEstimate
{
    public string Name { get; set; }

    public string Description { get; set; }

    public List<string> Assumptions { get; init; } = new();

    public List<FeatureEstimate> Features { get; init; } = new();


    public int? EstimatedTotal { get; set; }

    public int? WeightedTotal { get; set; }

    public double? EpicCost { get; set; }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Name);
        hash.Add(Description);
        hash.Add(EstimatedTotal);
        hash.Add(WeightedTotal);
        hash.Add(EpicCost);

        foreach (var assumption in Assumptions)
            hash.Add(assumption);

        foreach (var feature in Features)
            hash.Add(feature.GetHashCode());

        return hash.ToHashCode();
    }

}
