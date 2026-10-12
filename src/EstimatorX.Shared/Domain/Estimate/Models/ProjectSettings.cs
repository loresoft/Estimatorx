namespace EstimatorX.Domain.Models;

[Equatable, MessagePackObject(true)]
public partial class ProjectSettings
{
    public double? EstimateRate { get; set; }

    public int? UnitsPerWeek { get; set; } = 30;

    public bool ShowCost { get; set; }

    [SequenceEquality]
    public List<EffortLevel> EffortLevels { get; set; } = [];

    [SequenceEquality]
    public List<RiskLevel> RiskLevels { get; set; } = [];

    [SequenceEquality]
    public List<ProjectOverhead> Overhead { get; set; } = [];

    [SequenceEquality]
    public List<EstimateMultiplier> Multipliers { get; set; } = [];
}
