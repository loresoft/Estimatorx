using EstimatorX.Client.Extensions;
using EstimatorX.Client.Repositories;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class SettingsEditor<TStore, TRepository, TModel>
    where TStore : StoreEditBase<TModel, TRepository>
    where TRepository : RepositoryEditBase<TModel>
    where TModel : Project, IHaveIdentifier, new()
{

    private ProjectSettings ProjectSettings => Store?.Model?.Settings;


    private async Task RiskDelete(RiskLevel riskLevel)
    {
        if (!await Modal.ConfirmDelete())
            return;

        ProjectSettings.RiskLevels.Remove(riskLevel);
        Store.NotifyStateChanged();
    }

    private void RiskAdd()
    {
        ProjectSettings.RiskLevels.Add(new RiskLevel());
        Store.NotifyStateChanged();
    }


    private async Task EffortDelete(EffortLevel effortLevel)
    {
        if (!await Modal.ConfirmDelete())
            return;

        ProjectSettings.EffortLevels.Remove(effortLevel);
        Store.NotifyStateChanged();
    }

    private void EffortAdd()
    {
        var lastEffort = ProjectSettings.EffortLevels.LastOrDefault()
            ?? new EffortLevel { Effort = 1, Level = "" };

        ProjectSettings.EffortLevels
            .Add(new EffortLevel
            {
                Effort = lastEffort.Effort * 2,
                Level = lastEffort.Level
            });

        Store.NotifyStateChanged();
    }


    private async Task OverheadDelete(ProjectOverhead overhead)
    {
        if (!await Modal.ConfirmDelete())
            return;

        ProjectSettings.Overhead.Remove(overhead);
        Store.NotifyStateChanged();
    }

    private void OverheadAdd()
    {
        ProjectSettings.Overhead.Add(new ProjectOverhead());
        Store.NotifyStateChanged();
    }

}
