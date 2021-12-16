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
}
