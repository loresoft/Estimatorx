using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

[Equatable]
public partial class Invite : ModelBase, IHaveOrganization, IHaveName
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string OrganizationId { get; set; }

    public string OrganizationName { get; set; }

    public string SecurityKey { get; set; }
}
