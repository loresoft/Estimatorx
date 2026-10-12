namespace Estimatorx.Data.Cosmos.Models;


public partial class OrganizationMember : IdentifierName
{
    public string Email { get; set; }

    public bool IsOwner { get; set; }
}
