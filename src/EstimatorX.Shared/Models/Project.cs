using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

public class Project : ModelBase, IHaveOrganization, IHaveName
{
    public string Name { get; set; }

    public string Description { get; set; }


    public string OrganizationId { get; set; }

    public string OrganizationName { get; set; }


    public string SecurityKey { get; set; }


    public ProjectSettings Settings { get; init; } = new();

    public List<EpicEstimate> Epics { get; init; } = new();

    // computed
    public int? EstimatedTotal { get; set; }

    public int? WeightedTotal { get; set; }


    public double? EstimatedCost { get; set; }

    public double? WeightedCost { get; set; }


    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(base.GetHashCode());
        hash.Add(Name);
        hash.Add(Description);
        hash.Add(OrganizationId);
        hash.Add(SecurityKey);
        hash.Add(EstimatedTotal);
        hash.Add(WeightedTotal);
        hash.Add(EstimatedCost);
        hash.Add(WeightedCost);

        hash.Add(Settings.GetHashCode());

        foreach (var epic in Epics)
            hash.Add(epic.GetHashCode());

        return hash.ToHashCode();
    }
}
