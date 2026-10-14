using EstimatorX.Domain.Definitions;

namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class TemplateCreateModel : EntityCreateModel, IProjectEstimate
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public long? OrganizationId { get; set; }

    public long? UserId { get; set; }

    public ProjectSettings Settings { get; set; } = new();

    [SequenceEquality]
    public List<EpicEstimate> Epics { get; set; } = [];


    public int? EstimatedTotal { get; set; }

    public int? WeightedTotal { get; set; }


    public double? EstimatedWeeks { get; set; }

    public double? WeightedWeeks { get; set; }


    public decimal? EstimatedCost { get; set; }

    public decimal? WeightedCost { get; set; }
}
