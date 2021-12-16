namespace EstimatorX.Shared.Models;

public class StoryEstimate
{
    public string Name { get; set; }

    public string Description { get; set; }

    public int? Estimate { get; set; }

    public ClarityScale? Clarity { get; set; }

    public ConfidenceScale? Confidence { get; set; }

    public Criticality? Criticality { get; set; }

    public double? Multiplier { get; set; }

    public List<string> Assumptions { get; init; } = new();


    public int? WeightedEstimate { get; set; }

    public double? StoryCost { get; set; }
}
