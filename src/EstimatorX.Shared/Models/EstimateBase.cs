using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

public abstract class EstimateBase : IHaveEstimate
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string Assumptions { get; set; }


    public int? Estimate { get; set; }

    public ClarityScale? Clarity { get; set; }

    public ConfidenceScale? Confidence { get; set; }

    public Criticality? Criticality { get; set; }


    // computed
    public double? Multiplier { get; set; }

    public int? WeightedEstimate { get; set; }

    public int? EstimatedTotal { get; set; }

    public int? WeightedTotal { get; set; }


    public string RiskLevel { get; set; }

    public string EffortLevel { get; set; }


    public double? EstimateCost { get; set; }


    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Name);
        hash.Add(Description);
        hash.Add(Assumptions);
        hash.Add(Estimate);
        hash.Add(Clarity);
        hash.Add(Confidence);
        hash.Add(Criticality);

        hash.Add(Multiplier);
        hash.Add(WeightedEstimate);
        hash.Add(EstimatedTotal);
        hash.Add(WeightedTotal);
        hash.Add(RiskLevel);
        hash.Add(EffortLevel);
        hash.Add(EstimateCost);

        return hash.ToHashCode();
    }
}
