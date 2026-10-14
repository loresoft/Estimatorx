#pragma warning disable IDE0130 // Namespace does not match folder structure

using System.Diagnostics.CodeAnalysis;

using EstimatorX.Domain.Models;

namespace EstimatorX.Domain.Mapping;

[RegisterSingleton]
internal sealed class TemplateToProjectMapper : IMapper<TemplateReadModel, ProjectUpdateModel>
{
    [return: NotNullIfNotNull(nameof(source))]
    public ProjectUpdateModel? Map(TemplateReadModel? source)
    {
        if (source is null)
            return null;

        var destination = new ProjectUpdateModel();
        Map(source, destination);

        return destination;
    }

    public void Map(TemplateReadModel source, ProjectUpdateModel destination)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        destination.Settings = CloneSettings(source.Settings);
        destination.Epics = source.Epics.ConvertAll(CloneEpic);
        destination.EstimatedTotal = source.EstimatedTotal;
        destination.WeightedTotal = source.WeightedTotal;
        destination.EstimatedWeeks = source.EstimatedWeeks;
        destination.WeightedWeeks = source.WeightedWeeks;
        destination.EstimatedCost = source.EstimatedCost;
        destination.WeightedCost = source.WeightedCost;
    }

    public IQueryable<ProjectUpdateModel> ProjectTo(IQueryable<TemplateReadModel> source)
    {
        throw new NotSupportedException();
    }


    private static ProjectSettings CloneSettings(ProjectSettings? source)
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

    private static EpicEstimate CloneEpic(EpicEstimate source)
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

    private static FeatureEstimate CloneFeature(FeatureEstimate source)
    {
        var destination = new FeatureEstimate
        {
            Stories = source.Stories.ConvertAll(CloneStory),
        };

        CopyEstimate(source, destination);

        return destination;
    }

    private static StoryEstimate CloneStory(StoryEstimate source)
    {
        var destination = new StoryEstimate();
        CopyEstimate(source, destination);

        return destination;
    }

    private static void CopyEstimate(EstimateBase source, EstimateBase destination)
    {
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
