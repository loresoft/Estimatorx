namespace Estimatorx.Data.Cosmos.Models;


public partial class Invite : ModelBase
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string OrganizationId { get; set; }

    public string OrganizationName { get; set; }

    public string SecurityKey { get; set; }
}
