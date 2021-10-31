
using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Entities;

public class Organization : EntityBase
{
    public string Name { get; set; }

    public string Description { get; set; }

    public List<OrganizationMember> Members { get; set; } = new();
}
