using EstimatorX.Domain.Models;

namespace EstimatorX.Domain.Definitions;

public interface IProjectEstimate
{
    ProjectSettings Settings { get; set; }

    List<EpicEstimate> Epics { get; set; }

    int? EstimatedTotal { get; set; }

    int? WeightedTotal { get; set; }


    double? EstimatedWeeks { get; set; }

    double? WeightedWeeks { get; set; }


    decimal? EstimatedCost { get; set; }

    decimal? WeightedCost { get; set; }
}
