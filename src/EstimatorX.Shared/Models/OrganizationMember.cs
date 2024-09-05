namespace EstimatorX.Shared.Models;

[Equatable]
public partial class OrganizationMember : IdentifierName
{
    public string Email { get; set; }

    public bool IsOwner { get; set; }
}
