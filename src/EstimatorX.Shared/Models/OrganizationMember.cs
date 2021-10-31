namespace EstimatorX.Shared.Models;

public class OrganizationMember : IdentifierName
{
    public bool IsOwner { get; set; } = false;
}
