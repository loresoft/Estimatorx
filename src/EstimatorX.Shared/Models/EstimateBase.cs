using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

[Equatable]
public abstract partial class EstimateBase : IHaveIdentifier, IHaveName, IHaveEstimate
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

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


    public double? EstimatedCost { get; set; }

    public double? WeightedCost { get; set; }
}
