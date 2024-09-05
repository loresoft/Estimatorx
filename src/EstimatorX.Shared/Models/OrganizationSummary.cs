using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

[Equatable]
public partial class OrganizationSummary : ModelBase, IHaveName
{
    public string Name { get; set; }

    public string Description { get; set; }

}
