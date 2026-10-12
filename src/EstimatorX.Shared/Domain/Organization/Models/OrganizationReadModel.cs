using Arbiter.CommandQuery.Definitions;

namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class OrganizationReadModel : EntityReadModel, ISupportSearch
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [SequenceEquality]
    public IList<string>? HostMatches { get; set; }

    [SequenceEquality]
    public IList<long> Owners { get; set; } = [];

    public static IEnumerable<string> SearchFields()
        => [nameof(Name), nameof(Description)];

    public static string SortField()
        => nameof(Name);

    public override string ToString()
        => Name;

}
