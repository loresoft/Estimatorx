namespace EstimatorX.Shared.Models;

public class EstimateMultiplier
{
    public ConfidenceScale Confidence { get; set; }

    public ClarityScale Clarity { get; set; }

    public Criticality Criticality { get; set; }

    public double Value { get; set; }
}
