namespace EstimatorX.Shared.Models;

public class EstimateMultiplier
{
    public ConfidenceScale Confidence { get; set; }

    public ClarityScale Clarity { get; set; }

    public double Value { get; set; }


    public override int GetHashCode()
    {
        return HashCode.Combine(Confidence, Clarity, Value);
    }
}
