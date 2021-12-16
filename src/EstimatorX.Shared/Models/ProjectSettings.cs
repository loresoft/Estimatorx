namespace EstimatorX.Shared.Models;

public class ProjectSettings
{
    public double EstimateRate { get; set; }

    public List<int> EffortTimes { get; init; } = new();

    public List<RiskLevel> RiskLevels { get; init; } = new();

    public List<ProjectOverhead> Overhead { get; init; } = new();

    public List<EstimateMultiplier> Multipliers { get; init; } = new();
}
