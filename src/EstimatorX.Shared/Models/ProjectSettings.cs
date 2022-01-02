namespace EstimatorX.Shared.Models;

public class ProjectSettings
{
    public double EstimateRate { get; set; }

    public List<EffortLevel> EffortLevels { get; init; } = new();

    public List<RiskLevel> RiskLevels { get; init; } = new();

    public List<ProjectOverhead> Overhead { get; init; } = new();

    public List<EstimateMultiplier> Multipliers { get; init; } = new();


    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(EstimateRate);

        foreach (var effort in EffortLevels)
            hashCode.Add(effort.GetHashCode());

        foreach (var risk in RiskLevels)
            hashCode.Add(risk.GetHashCode());

        foreach (var overhead in Overhead)
            hashCode.Add(overhead.GetHashCode());

        foreach (var multiplier in Multipliers)
            hashCode.Add(multiplier.GetHashCode());

        return hashCode.ToHashCode();
    }
}
