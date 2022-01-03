using Blazored.Modal;
using Blazored.Modal.Services;

using EstimatorX.Client.Components;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class SettingsEditor
{
    [CascadingParameter]
    public IModalService Modal { get; set; }

    [Inject]
    public ProjectStore ProjectStore { get; set; }

    private ProjectSettings ProjectSettings => ProjectStore.Model.Settings;


    private async Task RiskDelete(RiskLevel riskLevel)
    {
        if (!await Confirm())
            return;

        ProjectSettings.RiskLevels.Remove(riskLevel);
        ProjectStore.NotifyStateChanged();
    }

    private void RiskAdd()
    {
        ProjectSettings.RiskLevels.Add(new RiskLevel());
        ProjectStore.NotifyStateChanged();
    }


    private async Task EffortDelete(EffortLevel effortLevel)
    {
        if (!await Confirm())
            return;

        ProjectSettings.EffortLevels.Remove(effortLevel);
        ProjectStore.NotifyStateChanged();
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

        ProjectStore.NotifyStateChanged();
    }


    private async Task OverheadDelete(ProjectOverhead overhead)
    {
        if (!await Confirm())
            return;

        ProjectSettings.Overhead.Remove(overhead);
        ProjectStore.NotifyStateChanged();
    }

    private void OverheadAdd()
    {
        ProjectSettings.Overhead.Add(new ProjectOverhead());
        ProjectStore.NotifyStateChanged();
    }

    private async Task<bool> Confirm()
    {
        var parameters = new ModalParameters();
        parameters.Add(nameof(ConfirmDelete.Message), "Are you sure you want to delete this?");

        var messageForm = Modal.Show<ConfirmDelete>("Confirm Delete", parameters);
        var result = await messageForm.Result;

        return !result.Cancelled;
    }
}
