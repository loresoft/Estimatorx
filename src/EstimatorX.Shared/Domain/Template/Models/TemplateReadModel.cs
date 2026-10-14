using System.ComponentModel.DataAnnotations.Schema;

using Arbiter.CommandQuery.Definitions;

using EstimatorX.Domain.Definitions;

namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class TemplateReadModel : EntityReadModel, IProjectEstimate, ISupportSearch
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public long? OrganizationId { get; set; }

    [Column("Organization.Name")]
    public string? OrganizationName { get; set; }

    public long? UserId { get; set; }

    [Column("User.Name")]
    public string? UserName { get; set; }

    [Column("User.Email")]
    public string? UserEmail { get; set; }

    public ProjectSettings Settings { get; set; } = new();

    [SequenceEquality]
    public List<EpicEstimate> Epics { get; set; } = [];

    public int? EstimatedTotal { get; set; }

    public int? WeightedTotal { get; set; }


    public double? EstimatedWeeks { get; set; }

    public double? WeightedWeeks { get; set; }


    public decimal? EstimatedCost { get; set; }

    public decimal? WeightedCost { get; set; }


    public static IEnumerable<string> SearchFields()
        => [nameof(Name), nameof(Description)];

    public static string SortField()
        => nameof(Name);

    public override string ToString()
        => Name;
}
