namespace EstimatorX.Shared.Models;

public class OrganizationModel : ModelBase
{
    public string Name { get; set; }

    public string Description { get; set; }

    public List<OrganizationMember> Members { get; set; } = new();


    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(base.GetHashCode());
        hashCode.Add(Name);
        hashCode.Add(Description);
        
        foreach (var member in Members)
            hashCode.Add(member.GetHashCode());

        return hashCode.ToHashCode();
    }
}
