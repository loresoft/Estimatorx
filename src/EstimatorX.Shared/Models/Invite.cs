
using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

public class Invite : ModelBase, IHaveOrganization, IHaveName
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string OrganizationId { get; set; }

    public string OrganizationName { get; set; }

    public string SecurityKey { get; set; }


    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Name, Email, OrganizationId, SecurityKey);
    }
}
