namespace EstimatorX.Shared.Models;

[Equatable]
public partial class EstimateMultiplier
{
    public ConfidenceScale Confidence { get; set; }

    public ClarityScale Clarity { get; set; }

    public double Value { get; set; }
}
