using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class SettingsEditor
{
    [Inject]
    public ProjectStore ProjectStore { get; set; }

    private ProjectSettings ProjectSettings => ProjectStore.Model.Settings;


    private void RiskDelete(RiskLevel riskLevel)
    {
        ProjectSettings.RiskLevels.Remove(riskLevel);
        ProjectStore.NotifyStateChanged();
    }

    private void RiskAdd()
    {
        ProjectSettings.RiskLevels.Add(new RiskLevel());
        ProjectStore.NotifyStateChanged();
    }


    private void EffortDelete(EffortLevel effortLevel)
    {
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


    private void OverheadDelete(ProjectOverhead overhead)
    {
        ProjectSettings.Overhead.Remove(overhead);
        ProjectStore.NotifyStateChanged();
    }

    private void OverheadAdd()
    {
        ProjectSettings.Overhead.Add(new ProjectOverhead());
        ProjectStore.NotifyStateChanged();
    }
}
