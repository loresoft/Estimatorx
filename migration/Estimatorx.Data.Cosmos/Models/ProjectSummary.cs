namespace Estimatorx.Data.Cosmos.Models;


public partial class ProjectSummary : ModelBase
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
