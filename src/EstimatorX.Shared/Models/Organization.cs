using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

[Equatable]
public partial class Organization : ModelBase, IHaveName
{
    public string Name { get; set; }

    public string Description { get; set; }

    [SequenceEquality]
    public List<OrganizationMember> Members { get; set; } = new();

    [SequenceEquality]
    public List<string> HostMatches { get; set; } = new();
}
