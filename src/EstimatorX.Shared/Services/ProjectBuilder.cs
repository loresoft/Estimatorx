
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Models;

namespace EstimatorX.Shared.Services;

public class ProjectBuilder : IProjectBuilder, ISingletonService
{
    public Project UpdateProject(Project project)
    {
        project ??= new Project();

        UpdateSettings(project.Settings);

        UpdateSamples(project);

        return project;
    }

    private void UpdateSamples(Project project)
    {
        if (project.Epics.Any())
            return;

        var epic = new EpicEstimate { Name = "Sample Epic Estimate" };

        var feature = new FeatureEstimate
        {
            Name = "Sample Feature Estimate",
            Estimate = project.Settings.EffortLevels.Select(e => e.Effort).FirstOrDefault(),
            Clarity = ClarityScale.High,
            Confidence = ConfidenceScale.High,
            Criticality = Criticality.Required,
        };

        epic.Features.Add(feature);

        project.Epics.Add(epic);
    }

    public ProjectSettings UpdateSettings(ProjectSettings settings)
    {
        settings ??= new ProjectSettings();

        if (settings.EstimateRate == 0)
            settings.EstimateRate = 100;

        if (settings.Multipliers.Count == 0)
        {
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.Low, Confidence = ConfidenceScale.Low, Value = 6 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.Low, Confidence = ConfidenceScale.MediumLow, Value = 5 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.Low, Confidence = ConfidenceScale.Medium, Value = 4 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.Low, Confidence = ConfidenceScale.MediumHigh, Value = 3 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.Low, Confidence = ConfidenceScale.High, Value = 2.5 });

            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.MediumLow, Confidence = ConfidenceScale.Low, Value = 5 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.MediumLow, Confidence = ConfidenceScale.MediumLow, Value = 4 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.MediumLow, Confidence = ConfidenceScale.Medium, Value = 3 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.MediumLow, Confidence = ConfidenceScale.MediumHigh, Value = 2.5 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.MediumLow, Confidence = ConfidenceScale.High, Value = 2 });

            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.Medium, Confidence = ConfidenceScale.Low, Value = 4 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.Medium, Confidence = ConfidenceScale.MediumLow, Value = 3 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.Medium, Confidence = ConfidenceScale.Medium, Value = 2.5 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.Medium, Confidence = ConfidenceScale.MediumHigh, Value = 2 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.Medium, Confidence = ConfidenceScale.High, Value = 1.5 });

            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.MediumHigh, Confidence = ConfidenceScale.Low, Value = 3 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.MediumHigh, Confidence = ConfidenceScale.MediumLow, Value = 2.5 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.MediumHigh, Confidence = ConfidenceScale.Medium, Value = 2 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.MediumHigh, Confidence = ConfidenceScale.MediumHigh, Value = 1.5 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.MediumHigh, Confidence = ConfidenceScale.High, Value = 1.25 });

            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.High, Confidence = ConfidenceScale.Low, Value = 2.5 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.High, Confidence = ConfidenceScale.MediumLow, Value = 2 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.High, Confidence = ConfidenceScale.Medium, Value = 1.5 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.High, Confidence = ConfidenceScale.MediumHigh, Value = 1.25 });
            settings.Multipliers.Add(new EstimateMultiplier { Clarity = ClarityScale.High, Confidence = ConfidenceScale.High, Value = 1 });
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
