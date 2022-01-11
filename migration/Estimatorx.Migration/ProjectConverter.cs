
using Estimatorx.Data.Mongo;

using EstimatorX.Shared.Services;

namespace Estimatorx.Migration;

public class ProjectConverter
{
    public ProjectBuilder ProjectBuilder { get; set; } = new();

    public EstimatorX.Shared.Services.ProjectCalculator ProjectCalculator { get; set; } = new();

    public EstimatorX.Shared.Models.Project Convert(Project source)
    {
        var target = new EstimatorX.Shared.Models.Project();

        // load default settings
        ProjectBuilder.UpdateSettings(target.Settings, true);

        target.Name = source.Name;
        target.Description = source.Description;

        target.Created = source.Created;
        target.CreatedBy = source.Creator;
        target.Updated = source.Updated;
        target.UpdatedBy = source.Updater;

        foreach (var section in source.Sections)
        {
            var epic = new EstimatorX.Shared.Models.EpicEstimate();
            epic.Name = section.Name;

            foreach (var task in section.Tasks)
            {
                var estimate = new EstimatorX.Shared.Models.FeatureEstimate();
                estimate.Name = task.Name;
                estimate.Criticality = EstimatorX.Shared.Models.Criticality.Required;

                var factory = source.Factors.FirstOrDefault(f => f.Id == task.FactorId);
                if (factory != null)
                    estimate.Name += $" - {factory.Name}";

                // find effort based on closest effort value
                var effortLevel = target.Settings.EffortLevels
                    .Select(r => new
                    {
                        Distance = Math.Abs(r.Effort - task.TotalHours),
                        Item = r
                    })
                    .MinBy(r => r.Distance);

                estimate.Estimate = effortLevel?.Item.Effort;

                if (task.VeryComplex > 0)
                {
                    estimate.Clarity = EstimatorX.Shared.Models.ClarityScale.Medium;
                    estimate.Confidence = EstimatorX.Shared.Models.ConfidenceScale.Medium;
                }
                else if (task.Complex > 0)
                {
                    estimate.Clarity = EstimatorX.Shared.Models.ClarityScale.Medium;
                    estimate.Confidence = EstimatorX.Shared.Models.ConfidenceScale.MediumHigh;
                }
                else if (task.Medium > 0)
                {
                    estimate.Clarity = EstimatorX.Shared.Models.ClarityScale.MediumHigh;
                    estimate.Confidence = EstimatorX.Shared.Models.ConfidenceScale.MediumHigh;
                }
                else if (task.Simple > 0)
                {
                    estimate.Clarity = EstimatorX.Shared.Models.ClarityScale.MediumHigh;
                    estimate.Confidence = EstimatorX.Shared.Models.ConfidenceScale.High;
                }
                else if (task.VerySimple > 0)
                {
                    estimate.Clarity = EstimatorX.Shared.Models.ClarityScale.High;
                    estimate.Confidence = EstimatorX.Shared.Models.ConfidenceScale.High;
                }

                epic.Features.Add(estimate);
            }

            target.Epics.Add(epic);
        }

        // update totals
        ProjectCalculator.UpdateProject(target);

        return target;
    }
}
