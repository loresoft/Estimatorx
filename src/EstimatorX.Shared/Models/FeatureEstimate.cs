namespace EstimatorX.Shared.Models;

public class FeatureEstimate
{
    public string Name { get; set; }

    public string Description { get; set; }

    public int? Estimate { get; set; }

    public ClarityScale? Clarity { get; set; }

    public ConfidenceScale? Confidence { get; set; }

    public Criticality? Criticality { get; set; }

    public double? Multiplier { get; set; }

    public List<string> Assumptions { get; init; } = new();

    public List<StoryEstimate> Stories { get; init; } = new();

    public int? WeightedEstimate { get; set; }


    public int? EstimatedTotal { get; set; }

    public int? WeightedTotal { get; set; }

    public double? FeatureCost { get; set;}
}
