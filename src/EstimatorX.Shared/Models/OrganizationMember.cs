namespace EstimatorX.Shared.Models;

public class OrganizationMember : IdentifierName
{
    public string Email { get; set; }

    public bool IsOwner { get; set; }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Email, IsOwner);
    }
}
