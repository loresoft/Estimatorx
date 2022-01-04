using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

public class ProjectSummary : ModelBase, IHaveName, IHaveOrganization
{
    public string Name { get; set; }

    public string Description { get; set; }


    public string OrganizationId { get; set; }

    public string OrganizationName { get; set; }

    // computed
    public int? EstimatedTotal { get; set; }

    public int? WeightedTotal { get; set; }


    public double? EstimatedCost { get; set; }

    public double? WeightedCost { get; set; }
}
