using EstimatorX.Shared.Models;

namespace EstimatorX.Shared.Definitions;

public interface IHaveEstimate
{
    // user entered
    ClarityScale? Clarity { get; set; }

    ConfidenceScale? Confidence { get; set; }

    Criticality? Criticality { get; set; }

    int? Estimate { get; set; }


    // computed
    double? Multiplier { get; set; }

    int? WeightedEstimate { get; set; }


    int? EstimatedTotal { get; set; }

    int? WeightedTotal { get; set; }


    string RiskLevel { get; set; }

    string EffortLevel { get; set; }


    double? EstimatedCost { get; set; }

    double? WeightedCost { get; set; }

}
