#pragma warning disable IDE0130 // Namespace does not match folder structure

using System.Diagnostics.CodeAnalysis;

using EstimatorX.Domain.Models;

namespace EstimatorX.Domain.Mapping;

internal abstract class EstimateMapper<TModel> : IMapper<TModel, TModel>
    where TModel : class, new()
{
    [return: NotNullIfNotNull(nameof(source))]
    public TModel? Map(TModel? source)
    {
        if (source is null)
            return null;

        var destination = new TModel();
        Map(source, destination);

        return destination;
    }

    public abstract void Map(TModel source, TModel destination);

    public IQueryable<TModel> ProjectTo(IQueryable<TModel> source) => source;
}

public static class EstimateMapper
{
    public static ProjectSettings CloneSettings(ProjectSettings? source)
    {
        if (source is null)
            return new();

        return new ProjectSettings
        {
            EstimateRate = source.EstimateRate,
            UnitsPerWeek = source.UnitsPerWeek,
            ShowCost = source.ShowCost,
            EffortLevels = source.EffortLevels.ConvertAll(e => new EffortLevel
            {
                Effort = e.Effort,
                Level = e.Level,
            }),
            RiskLevels = source.RiskLevels.ConvertAll(r => new RiskLevel
            {
                Risk = r.Risk,
                Multiplier = r.Multiplier,
            }),
            Overhead = source.Overhead.ConvertAll(o => new ProjectOverhead
            {
                Name = o.Name,
                Description = o.Description,
                Multiplier = o.Multiplier,
            }),
            Multipliers = source.Multipliers.ConvertAll(m => new EstimateMultiplier
            {
                Confidence = m.Confidence,
                Clarity = m.Clarity,
                Value = m.Value,
            }),
        };
    }

    public static EpicEstimate CloneEpic(EpicEstimate source)
    {
        return new EpicEstimate
        {
            Name = source.Name,
            Description = source.Description,
            Assumptions = source.Assumptions,
            Features = source.Features.ConvertAll(CloneFeature),
            EstimatedTotal = source.EstimatedTotal,
            WeightedTotal = source.WeightedTotal,
            EstimatedWeeks = source.EstimatedWeeks,
            WeightedWeeks = source.WeightedWeeks,
            EstimatedCost = source.EstimatedCost,
            WeightedCost = source.WeightedCost,
        };
    }

    public static FeatureEstimate CloneFeature(FeatureEstimate source)
    {
        var destination = new FeatureEstimate();
        CopyEstimate(source, destination);

        destination.Stories = source.Stories.ConvertAll(CloneStory);

        return destination;
    }

    public static StoryEstimate CloneStory(StoryEstimate source)
    {
        var destination = new StoryEstimate();
        CopyEstimate(source, destination);

        return destination;
    }

    public static void CopyEstimate(EstimateBase source, EstimateBase destination)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.Name = source.Name;
        destination.Description = source.Description;
        destination.Assumptions = source.Assumptions;

        destination.Estimate = source.Estimate;
        destination.Clarity = source.Clarity;
        destination.Confidence = source.Confidence;
        destination.Criticality = source.Criticality;

        destination.Multiplier = source.Multiplier;
        destination.WeightedEstimate = source.WeightedEstimate;
        destination.EstimatedTotal = source.EstimatedTotal;
        destination.WeightedTotal = source.WeightedTotal;
        destination.EstimatedWeeks = source.EstimatedWeeks;
        destination.WeightedWeeks = source.WeightedWeeks;

        destination.RiskLevel = source.RiskLevel;
        destination.EffortLevel = source.EffortLevel;

        destination.EstimatedCost = source.EstimatedCost;
        destination.WeightedCost = source.WeightedCost;
    }
}
