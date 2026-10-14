namespace Estimatorx.Data.Cosmos.Models;


public partial class Organization : ModelBase
{
    public string Name { get; set; }

    public string Description { get; set; }


    public List<OrganizationMember> Members { get; set; } = new();

    public List<string> HostMatches { get; set; } = new();

    public long? Key { get; set; }
}
