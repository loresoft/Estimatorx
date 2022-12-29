using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

public class Organization : ModelBase, IHaveName
{
    public string Name { get; set; }

    public string Description { get; set; }

    public List<OrganizationMember> Members { get; init; } = new();

    public List<string> HostMatches { get; init; } = new();


    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(base.GetHashCode());
        hashCode.Add(Name);
        hashCode.Add(Description);
        
        foreach (var member in Members)
            hashCode.Add(member.GetHashCode());

        foreach (var host in HostMatches)
            hashCode.Add(host.GetHashCode());

        return hashCode.ToHashCode();
    }
}
