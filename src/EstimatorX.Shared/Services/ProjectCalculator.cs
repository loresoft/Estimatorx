using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Models;


namespace EstimatorX.Shared.Services;

public class ProjectCalculator : IProjectCalculator, ISingletonService
{
    public void UpdateProject(Project project)
    {
        UpdateSettings(project);

        project.EstimatedTotal = 0;
        project.WeightedTotal = 0;
        project.EstimatedCost = 0;
        project.WeightedCost = 0;

        foreach (var epic in project.Epics)
        {
            UpdateEpic(project, epic);

            // update parent totals
            project.EstimatedTotal += epic.EstimatedTotal;
            project.WeightedTotal += epic.WeightedTotal;
            project.EstimatedCost += epic.EstimatedCost;
            project.WeightedCost += epic.WeightedCost;

        }
    }

    public void UpdateEpic(Project project, EpicEstimate epic)
    {
        epic.EstimatedTotal = 0;
        epic.WeightedTotal = 0;
        epic.EstimatedCost = 0;
        epic.WeightedCost = 0;

        foreach (var feature in epic.Features)
        {
            UpdateFeature(project, feature);

            // update parent totals
            epic.EstimatedTotal += feature.EstimatedTotal;
            epic.WeightedTotal += feature.WeightedTotal;
            epic.EstimatedCost += feature.EstimatedCost;
            epic.WeightedCost += feature.WeightedCost;
        }
    }

    public void UpdateFeature(Project project, FeatureEstimate feature)
    {
        UpdateWeightedEstimate(project, feature);
        UpdateRisk(project, feature);
        UpdateEffort(project, feature);
        UpdateTotals(project, feature);

        foreach (var story in feature.Stories)
        {
            UpdateStory(project, story);

            // update parent totals
            feature.EstimatedTotal += story.Estimate;
            feature.WeightedTotal += story.WeightedEstimate;
            feature.EstimatedCost += story.EstimatedCost;
            feature.WeightedCost += story.WeightedCost;
        }
    }

    public void UpdateSettings(Project project)
    {
        project.Settings.RiskLevels.Sort((x, y) => x.Multiplier.CompareTo(y.Multiplier));
        project.Settings.EffortLevels.Sort((x, y) => x.Effort.CompareTo(y.Effort));
    }

    private void UpdateStory(Project project, StoryEstimate story)
    {
        UpdateWeightedEstimate(project, story);
        UpdateRisk(project, story);
        UpdateEffort(project, story);
        UpdateTotals(project, story);
    }

    private static void UpdateTotals(Project project, IHaveEstimate estimate)
    {
        double estimatedTotal = estimate.Estimate ?? 0;
        double weightedTotal = estimate.WeightedEstimate ?? 0;

        foreach (var overhead in project.Settings.Overhead)
        {
            estimatedTotal *= overhead.Multiplier;
            weightedTotal *= overhead.Multiplier;
        }

        // always round up
        estimate.EstimatedTotal = (int)Math.Round(estimatedTotal, 0, MidpointRounding.AwayFromZero);
        estimate.WeightedTotal = (int)Math.Round(weightedTotal, 0, MidpointRounding.AwayFromZero);

        estimate.EstimatedCost = estimate.EstimatedTotal * project.Settings.EstimateRate;
        estimate.WeightedCost = estimate.WeightedTotal * project.Settings.EstimateRate;
    }

    private static void UpdateRisk(Project project, IHaveEstimate estimate)
    {
        if (!estimate.Multiplier.HasValue || estimate.Multiplier.Value == 0)
        {
            estimate.RiskLevel = null;
            return;
        }

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
        {
            estimate.EffortLevel = null;
            return;
        }

        var value = estimate.Estimate.Value;

        // find effort based on closest effort value
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
        {
            estimate.Multiplier = null;
            estimate.WeightedEstimate = null;
            return;
        }


        estimate.Multiplier = GetWeightedMultiplier(project, estimate);

        var weighted = estimate.Estimate * estimate.Multiplier;
        if (!weighted.HasValue)
            return;

        // always round up
        estimate.WeightedEstimate = (int)Math.Round(weighted.Value, 0, MidpointRounding.AwayFromZero);
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
