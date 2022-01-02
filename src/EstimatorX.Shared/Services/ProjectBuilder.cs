
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Models;

namespace EstimatorX.Shared.Services;

public class ProjectBuilder : IProjectBuilder, ISingletonService
{
    public Project Build(Project project)
    {
        project ??= new Project();

        UpdateSettings(project.Settings);

        return project;
    }

    public ProjectSettings UpdateSettings(ProjectSettings settings)
    {
        settings ??= new ProjectSettings();

        if (settings.EstimateRate == 0)
            settings.EstimateRate = 100;

        if (settings.Multipliers.Count == 0)
        {
            var multipliers = from clarityScale in Enum.GetValues<ClarityScale>()
                              from confidenceScale in Enum.GetValues<ConfidenceScale>()
                              let multiplier = new EstimateMultiplier
                              {
                                  Clarity = clarityScale,
                                  Confidence = confidenceScale,
                              }
                              select multiplier;

            settings.Multipliers.AddRange(multipliers);
        }

        if (settings.RiskLevels.Count == 0)
        {
            settings.RiskLevels.Add(new RiskLevel { Multiplier = 1, Risk = "Minimal" });
            settings.RiskLevels.Add(new RiskLevel { Multiplier = 1.25, Risk = "Low" });
            settings.RiskLevels.Add(new RiskLevel { Multiplier = 1.5, Risk = "Medium Low" });
            settings.RiskLevels.Add(new RiskLevel { Multiplier = 2, Risk = "Medium" });
            settings.RiskLevels.Add(new RiskLevel { Multiplier = 2.5, Risk = "Medium High" });
            settings.RiskLevels.Add(new RiskLevel { Multiplier = 3, Risk = "High" });
            settings.RiskLevels.Add(new RiskLevel { Multiplier = 4, Risk = "Needs Refinement" });
        }

        if (settings.EffortLevels.Count == 0)
        {
            settings.EffortLevels.Add(new EffortLevel { Effort = 2, Level = "Minimal" });
            settings.EffortLevels.Add(new EffortLevel { Effort = 4, Level = "Low" });
            settings.EffortLevels.Add(new EffortLevel { Effort = 8, Level = "Medium Low" });
            settings.EffortLevels.Add(new EffortLevel { Effort = 16, Level = "Medium" });
            settings.EffortLevels.Add(new EffortLevel { Effort = 24, Level = "Medium High" });
            settings.EffortLevels.Add(new EffortLevel { Effort = 48, Level = "High" });
        }

        if (settings.Overhead.Count == 0)
        {
            settings.Overhead.Add(new ProjectOverhead { Name = "Unit Tests", Multiplier = 1.1, Description = "Overhead to create unit tests" });
            settings.Overhead.Add(new ProjectOverhead { Name = "Quality Assurance", Multiplier = 1.1, Description = "Overhead for quality assurance testing" });
            settings.Overhead.Add(new ProjectOverhead { Name = "Project Management", Multiplier = 1.1, Description = "Overhead for project management activities" });
            settings.Overhead.Add(new ProjectOverhead { Name = "Meetings", Multiplier = 1.05, Description = "Overhead for team meeetings" });
        }

        return settings;
    }
}
