using Arbiter.CommandQuery.Definitions;

using EstimatorX.Domain.Definitions;
using EstimatorX.Domain.Models;

namespace EstimatorX.Data.Entities;

public class Template : IHaveIdentifier<long>, ITrackCreated, ITrackUpdated, IProjectEstimate
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public long? OrganizationId { get; set; }

    public long? UserId { get; set; }

    public ProjectSettings Settings { get; set; } = new();

    public List<EpicEstimate> Epics { get; set; } = [];


    public int? EstimatedTotal { get; set; }

    public int? WeightedTotal { get; set; }


    public double? EstimatedWeeks { get; set; }

    public double? WeightedWeeks { get; set; }


    public decimal? EstimatedCost { get; set; }

    public decimal? WeightedCost { get; set; }


    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTimeOffset Updated { get; set; } = DateTimeOffset.UtcNow;

    public string? UpdatedBy { get; set; }


    public virtual Organization? Organization { get; set; }

    public virtual User? User { get; set; }
}
