using System.ComponentModel.DataAnnotations.Schema;

using Arbiter.CommandQuery.Definitions;

namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class InviteReadModel : EntityReadModel, ISupportSearch
{
    public string Email { get; set; } = null!;

    public string? Name { get; set; }

    public long OrganizationId { get; set; }

    [Column("Organization.Name")]
    public string OrganizationName { get; set; } = null!;

    public string? SecurityCode { get; set; }

    public DateTimeOffset Expiration { get; set; }

    public DateTimeOffset? LastSent { get; set; }


    public static IEnumerable<string> SearchFields()
    => [nameof(Email), nameof(Name)];

    public static string SortField()
        => nameof(Email);

    public override string ToString()
        => $"{Name} {Email}";

}
