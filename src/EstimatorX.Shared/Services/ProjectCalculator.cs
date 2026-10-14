using EstimatorX.Domain.Definitions;
using EstimatorX.Domain.Models;

namespace EstimatorX.Services;

[RegisterSingleton]
public class ProjectCalculator : IProjectCalculator
{
    public void UpdateProject(IProjectEstimate project)
    {
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
            project.EstimatedCost += (decimal?)epic.EstimatedCost;
            project.WeightedCost += (decimal?)epic.WeightedCost;

        }

        project.EstimatedWeeks = CalculateWeeks(project, project.EstimatedTotal);
        project.WeightedWeeks = CalculateWeeks(project, project.WeightedTotal);
    }

    public void UpdateEpic(IProjectEstimate project, EpicEstimate epic)
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

        epic.EstimatedWeeks = CalculateWeeks(project, epic.EstimatedTotal);
        epic.WeightedWeeks = CalculateWeeks(project, epic.WeightedTotal);
    }

    public void UpdateFeature(IProjectEstimate project, FeatureEstimate feature)
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

        feature.EstimatedWeeks = CalculateWeeks(project, feature.EstimatedTotal);
        feature.WeightedWeeks = CalculateWeeks(project, feature.WeightedTotal);
    }

    public void UpdateSettings(IProjectEstimate project)
    {
        project.Settings.RiskLevels.Sort((x, y) => x.Multiplier.CompareTo(y.Multiplier));
        project.Settings.EffortLevels.Sort((x, y) => x.Effort.CompareTo(y.Effort));
    }

    private void UpdateStory(IProjectEstimate project, StoryEstimate story)
    {
        UpdateWeightedEstimate(project, story);
        UpdateRisk(project, story);
        UpdateEffort(project, story);
        UpdateTotals(project, story);
    }


    private static void UpdateTotals(IProjectEstimate project, IHaveEstimate estimate)
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

        estimate.EstimatedWeeks = CalculateWeeks(project, estimate.EstimatedTotal);
        estimate.WeightedWeeks = CalculateWeeks(project, estimate.WeightedTotal);
    }

    private static double? CalculateWeeks(IProjectEstimate project, int? total)
    {
        var unitsPerWeek = project.Settings.UnitsPerWeek;
        if (!total.HasValue || !unitsPerWeek.HasValue || unitsPerWeek.Value <= 0)
            return null;

        return Math.Round((double)total.Value / unitsPerWeek.Value, 1, MidpointRounding.AwayFromZero);
    }

    private static void UpdateRisk(IProjectEstimate project, IHaveEstimate estimate)
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

    private static void UpdateEffort(IProjectEstimate project, IHaveEstimate estimate)
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

    private static void UpdateWeightedEstimate(IProjectEstimate project, IHaveEstimate estimate)
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

    private static double GetWeightedMultiplier(IProjectEstimate project, IHaveEstimate estimate)
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
