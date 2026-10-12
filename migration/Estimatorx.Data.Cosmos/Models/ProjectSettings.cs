namespace Estimatorx.Data.Cosmos.Models;


public partial class ProjectSettings
{
    public double EstimateRate { get; set; }

    
    public List<EffortLevel> EffortLevels { get; set; } = new();

    
    public List<RiskLevel> RiskLevels { get; set; } = new();

    
    public List<ProjectOverhead> Overhead { get; set; } = new();

    
    public List<EstimateMultiplier> Multipliers { get; set; } = new();
}
