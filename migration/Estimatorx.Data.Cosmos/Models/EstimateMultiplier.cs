namespace Estimatorx.Data.Cosmos.Models;


public partial class EstimateMultiplier
{
    public ConfidenceScale Confidence { get; set; }

    public ClarityScale Clarity { get; set; }

    public double Value { get; set; }
}
