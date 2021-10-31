namespace EstimatorX.Shared.Models;

public class OrganizationModel : ModelBase
{
    public string Name { get; set; }

    public string Description { get; set; }

    public HashSet<OrganizationMember> Members { get; set; } = new();
}
