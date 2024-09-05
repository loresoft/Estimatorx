using EstimatorX.Shared.Definitions;

namespace EstimatorX.Shared.Models;

[Equatable]
public partial class ProjectOverhead : IHaveName
{
    public string Name { get; set; }

    public string Description { get; set; }

    public double Multiplier { get; set; }
}
