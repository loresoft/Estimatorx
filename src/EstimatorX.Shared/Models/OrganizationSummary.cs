using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

public class OrganizationSummary : ModelBase, IHaveName
{
    public string Name { get; set; }

    public string Description { get; set; }

}
