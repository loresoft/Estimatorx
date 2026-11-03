using EstimatorX.Domain.Definitions;
using EstimatorX.Domain.Models;

namespace EstimatorX.Services;

[RegisterSingleton]
public class ProjectCalculator : IProjectCalculator
{
    public void UpdateProject(IProjectEstimate project)
    {
        var totals = EstimateTotals.Zero;

        foreach (var epic in project.Epics)
            totals += CalculateEpic(project, epic);

        project.EstimatedTotal = RoundTotal(totals.Estimated);
        project.WeightedTotal = RoundTotal(totals.Weighted);

        project.EstimatedCost = (decimal?)CalculateCost(project, project.EstimatedTotal);
        project.WeightedCost = (decimal?)CalculateCost(project, project.WeightedTotal);

        project.EstimatedWeeks = CalculateWeeks(project, project.EstimatedTotal);
        project.WeightedWeeks = CalculateWeeks(project, project.WeightedTotal);
    }

    public void UpdateEpic(IProjectEstimate project, EpicEstimate epic)
    {
        CalculateEpic(project, epic);
    }

    public void UpdateFeature(IProjectEstimate project, FeatureEstimate feature)
    {
        CalculateFeature(project, feature);
    }

    public void UpdateSettings(IProjectEstimate project)
    {
        project.Settings.RiskLevels.Sort((x, y) => x.Multiplier.CompareTo(y.Multiplier));
        project.Settings.EffortLevels.Sort((x, y) => x.Effort.CompareTo(y.Effort));
    }


    private static EstimateTotals CalculateEpic(IProjectEstimate project, EpicEstimate epic)
    {
        var totals = EstimateTotals.Zero;

        foreach (var feature in epic.Features)
            totals += CalculateFeature(project, feature);

        epic.EstimatedTotal = RoundTotal(totals.Estimated);
        epic.WeightedTotal = RoundTotal(totals.Weighted);

        epic.EstimatedCost = CalculateCost(project, epic.EstimatedTotal);
        epic.WeightedCost = CalculateCost(project, epic.WeightedTotal);

        epic.EstimatedWeeks = CalculateWeeks(project, epic.EstimatedTotal);
        epic.WeightedWeeks = CalculateWeeks(project, epic.WeightedTotal);

        return totals;
    }

    private static EstimateTotals CalculateFeature(IProjectEstimate project, FeatureEstimate feature)
    {
        var totals = CalculateEstimate(project, feature);

        foreach (var story in feature.Stories)
            totals += CalculateEstimate(project, story);

        ApplyTotals(project, feature, totals);

        return totals;
    }

    private static EstimateTotals CalculateEstimate(IProjectEstimate project, IHaveEstimate estimate)
    {
        UpdateWeightedEstimate(project, estimate);
        UpdateRisk(project, estimate);
        UpdateEffort(project, estimate);

        var totals = CalculateTotals(project, estimate);
        ApplyTotals(project, estimate, totals);

        return totals;
    }

    private static EstimateTotals CalculateTotals(IProjectEstimate project, IHaveEstimate estimate)
    {
        if (!estimate.Estimate.HasValue || estimate.Estimate.Value == 0)
            return EstimateTotals.Zero;

        // overhead is applied to the base estimate only, not compounded
        var overheadFactor = GetOverheadFactor(project);
        var multiplier = estimate.Multiplier ?? 1;

        // use unrounded values so rounding only happens once per level
        var estimated = estimate.Estimate.Value * overheadFactor;
        var weighted = estimate.Estimate.Value * multiplier * overheadFactor;

        return new EstimateTotals(estimated, weighted);
    }


    private static void ApplyTotals(IProjectEstimate project, IHaveEstimate estimate, EstimateTotals totals)
    {
        estimate.EstimatedTotal = RoundTotal(totals.Estimated);
        estimate.WeightedTotal = RoundTotal(totals.Weighted);

        estimate.EstimatedCost = CalculateCost(project, estimate.EstimatedTotal);
        estimate.WeightedCost = CalculateCost(project, estimate.WeightedTotal);

        estimate.EstimatedWeeks = CalculateWeeks(project, estimate.EstimatedTotal);
        estimate.WeightedWeeks = CalculateWeeks(project, estimate.WeightedTotal);
    }

    private static double GetOverheadFactor(IProjectEstimate project)
    {
        // overhead is applied to the base estimate only, not compounded
        // e.g. if overhead is 10% and 20%, the factor is 1 + (0.1 + 0.2) = 1.3
        return 1 + project.Settings.Overhead.Sum(o => o.Multiplier - 1);
    }

    private static int RoundTotal(double value)
    {
        return (int)Math.Round(value, 0, MidpointRounding.AwayFromZero);
    }

    private static double? CalculateCost(IProjectEstimate project, int? total)
    {
        return total * project.Settings.EstimateRate;
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

        // compute weighted estimate
        estimate.Multiplier = GetWeightedMultiplier(project, estimate);

        var weighted = estimate.Estimate * estimate.Multiplier;
        if (!weighted.HasValue)
            return;

        estimate.WeightedEstimate = RoundTotal(weighted.Value);
    }

    private static double GetWeightedMultiplier(IProjectEstimate project, IHaveEstimate estimate)
    {
        // compute multiplier by looking up the multiplier for the given clarity and confidence levels
        var multiplier = project.Settings.Multipliers
            .Where(m => m.Clarity == estimate.Clarity && m.Confidence == estimate.Confidence)
            .Select(m => m.Value)
            .FirstOrDefault();

        // make sure at least 1
        return Math.Max(multiplier, 1);
    }

    // record struct to hold totals for estimated and weighted values
    private readonly record struct EstimateTotals(double Estimated, double Weighted)
    {
        public static EstimateTotals Zero => new(0, 0);

        public static EstimateTotals operator +(EstimateTotals left, EstimateTotals right)
        {
            return new(
                Estimated: left.Estimated + right.Estimated,
                Weighted: left.Weighted + right.Weighted
            );
        }
    }
}
