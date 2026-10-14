using EstimatorX.Domain.Definitions;

namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class ProjectOverhead : IHaveName
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public double Multiplier { get; set; } = 1;

    public override string ToString() => Name;
}
