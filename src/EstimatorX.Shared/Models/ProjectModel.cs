using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

public class ProjectModel : ModelBase, IHaveOrganization
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string OrganizationId { get; set; }

    public string SecurityKey { get; set; }
        

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(base.GetHashCode());
        hash.Add(Name);
        hash.Add(Description);
        hash.Add(OrganizationId);
        hash.Add(SecurityKey);

        return hash.ToHashCode();
    }
}
