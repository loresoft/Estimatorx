namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class EstimateMultiplier
{
    public ConfidenceScale Confidence { get; set; } = ConfidenceScale.MediumHigh;

    public ClarityScale Clarity { get; set; } = ClarityScale.MediumHigh;

    public double Value { get; set; } = 1;

    public override string ToString() => $"Confidence: {Confidence}, Clarity: {Clarity}, Multiplier: {Value}";
}
