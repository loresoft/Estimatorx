namespace EstimatorX.Shared.Models;

[Equatable]
public partial class ProjectSettings
{
    public double EstimateRate { get; set; }

    [SequenceEquality]
    public List<EffortLevel> EffortLevels { get; set; } = new();

    [SequenceEquality]
    public List<RiskLevel> RiskLevels { get; set; } = new();

    [SequenceEquality]
    public List<ProjectOverhead> Overhead { get; set; } = new();

    [SequenceEquality]
    public List<EstimateMultiplier> Multipliers { get; set; } = new();
}
