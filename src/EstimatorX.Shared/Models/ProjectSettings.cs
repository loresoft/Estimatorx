namespace EstimatorX.Shared.Models;

public class ProjectSettings
{
    public double EstimateRate { get; set; }

    public List<EffortLevel> EffortLevels { get; set; } = new();

    public List<RiskLevel> RiskLevels { get; set; } = new();

    public List<ProjectOverhead> Overhead { get; set; } = new();

    public List<EstimateMultiplier> Multipliers { get; set; } = new();


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
