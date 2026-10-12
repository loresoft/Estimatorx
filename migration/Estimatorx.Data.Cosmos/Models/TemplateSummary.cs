namespace Estimatorx.Data.Cosmos.Models;


public partial class TemplateSummary : ModelBase
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string OrganizationId { get; set; }

    public string OrganizationName { get; set; }
}
