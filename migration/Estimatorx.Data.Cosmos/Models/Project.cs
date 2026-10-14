namespace Estimatorx.Data.Cosmos.Models;


public partial class Project : ModelBase
{
    public string Name { get; set; }

    public string Description { get; set; }


    public string OrganizationId { get; set; }

    public string OrganizationName { get; set; }


    public string SecurityKey { get; set; }

    public string TemplateKey { get; set; }


    public ProjectSettings Settings { get; set; } = new();


    public List<EpicEstimate> Epics { get; set; } = new();


    // computed
    public int? EstimatedTotal { get; set; }

    public int? WeightedTotal { get; set; }


    public double? EstimatedCost { get; set; }

    public double? WeightedCost { get; set; }

    public long? Key { get; set; }
}
