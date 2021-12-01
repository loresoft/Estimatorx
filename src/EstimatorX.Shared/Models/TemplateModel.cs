using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

public class TemplateModel : ModelBase, IHaveOrganization
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string OrganizationId { get; set; }

    public string OrganizationName { get; set; }


    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(base.GetHashCode());
        hash.Add(Name);
        hash.Add(Description);
        hash.Add(OrganizationId);

        return hash.ToHashCode();
    }
}
