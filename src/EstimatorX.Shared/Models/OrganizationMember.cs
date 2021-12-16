namespace EstimatorX.Shared.Models;

public class OrganizationMember : IdentifierName
{
    public bool IsOwner { get; set; }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), IsOwner);
    }
}
