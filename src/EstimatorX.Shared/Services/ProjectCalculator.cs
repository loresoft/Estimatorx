using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Models;


namespace EstimatorX.Shared.Services;

public class ProjectCalculator
{
    public void UpdateProject(Project project)
    {
        foreach (var epic in project.Epics)
        {
            UpdateEpic(project, epic);

            // update parent totals
            project.EstimatedTotal += epic.EstimatedTotal;
            project.WeightedTotal += epic.WeightedTotal;
        }
    }

    public void UpdateEpic(Project project, EpicEstimate epic)
    {
        foreach (var feature in epic.Features)
        {
            UpdateFeature(project, feature);

            // update parent totals
            epic.EstimatedTotal += feature.EstimatedTotal;
            epic.WeightedTotal += feature.WeightedTotal;
        }
    }

    public void UpdateFeature(Project project, FeatureEstimate feature)
    {
        if (feature.Estimate.HasValue)
        {
            UpdateWeightedEstimate(project, feature);
            UpdateRisk(project, feature);
            UpdateEffort(project, feature);

            feature.EstimatedTotal = feature.Estimate;
            feature.WeightedTotal = feature.WeightedEstimate;
        }

        foreach (var story in feature.Stories)
        {
            UpdateStory(project, story);

            // update parent totals
            feature.EstimatedTotal += story.Estimate;
            feature.WeightedTotal += story.WeightedEstimate;
        }
    }

    private void UpdateStory(Project project, StoryEstimate story)
    {
        UpdateWeightedEstimate(project, story);
        UpdateRisk(project, story);
        UpdateEffort(project, story);
    }

    private static void UpdateRisk(Project project, IHaveEstimate estimate)
    {
        if (!estimate.Multiplier.HasValue || estimate.Multiplier.Value == 0)
            return;

        var multiplier = estimate.Multiplier.Value;
        
        // find risk based on closest multiplier
        var risk = project.Settings.RiskLevels
            .Select(r => new
            {
                Distance = Math.Abs(r.Multiplier - multiplier),
                Risk = r
            })
            .MinBy(r => r.Distance);

        estimate.RiskLevel = risk?.Risk?.Risk;
    }

    private static void UpdateEffort(Project project, IHaveEstimate estimate)
    {
        if (!estimate.Estimate.HasValue || estimate.Estimate.Value == 0)
            return;

        var value = estimate.Estimate.Value;

        // find risk based on closest effort value
        var effort = project.Settings.EffortLevels
            .Select(r => new
            {
                Distance = Math.Abs(r.Effort - value),
                Effort = r
            })
            .MinBy(r => r.Distance);

        estimate.EffortLevel = effort?.Effort?.Level;
    }

    private static void UpdateWeightedEstimate(Project project, IHaveEstimate estimate)
    {
        if (!estimate.Estimate.HasValue || estimate.Estimate.Value == 0)
            return;

        estimate.Multiplier = GetWeightedMultiplier(project, estimate);

        var weighted = estimate.Estimate * estimate.Multiplier;
        if (!weighted.HasValue)
            return;

        // always round up
        estimate.WeightedEstimate = (int)Math.Ceiling(weighted.Value);
    }


    private static double GetWeightedMultiplier(Project project, IHaveEstimate estimate)
    {
        // compute multiplier
        var multiplier = project.Settings.Multipliers
            .Where(m => m.Clarity == estimate.Clarity && m.Confidence == estimate.Confidence && m.Clarity == estimate.Clarity)
            .Select(m => m.Value)
            .FirstOrDefault();

        // make sure at least 1
        multiplier = Math.Max(multiplier, 1);

        return multiplier;
    }
}
