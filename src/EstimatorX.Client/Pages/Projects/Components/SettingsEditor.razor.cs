using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class SettingsEditor
{
    [Parameter]
    public ProjectSettings ProjectSettings { get; set; } = new();

    [Parameter]
    public EventCallback ProjectSettingsChanged { get; set; }


    private void MultiplierUpdate(EstimateMultiplier multiplier, ChangeEventArgs change)
    {
        if (multiplier == null || change == null)
            return;

        var value = change.Value?.ToString()?.ToDouble();
        multiplier.Value = value ?? 0;

        ProjectSettingsChanged.InvokeAsync();
    }


    private void RiskUpdateMultiplier(RiskLevel riskLevel, ChangeEventArgs change)
    {
        if (riskLevel == null || change == null)
            return;

        var value = change.Value?.ToString()?.ToDouble();
        riskLevel.Multiplier = value ?? 0;

        ProjectSettingsChanged.InvokeAsync();
    }


    private void RiskUpdateText(RiskLevel riskLevel, ChangeEventArgs change)
    {
        if (riskLevel == null || change == null)
            return;

        riskLevel.Risk = change.Value?.ToString();

        ProjectSettingsChanged.InvokeAsync();
    }

    private void RiskDelete(RiskLevel riskLevel)
    {
        ProjectSettings.RiskLevels.Remove(riskLevel);

        ProjectSettingsChanged.InvokeAsync();
    }

    private void RiskAdd()
    {
        ProjectSettings.RiskLevels.Add(new RiskLevel());

        ProjectSettingsChanged.InvokeAsync();
    }


    private void EffortUpdateValue(EffortLevel effortLevel, ChangeEventArgs change)
    {
        if (change == null)
            return;

        var value = change.Value?.ToString()?.ToInt32();
        effortLevel.Effort = value ?? 0;

        ProjectSettingsChanged.InvokeAsync();
    }

    private void EffortUpdateLevel(EffortLevel effortLevel, ChangeEventArgs change)
    {
        if (change == null)
            return;

        var value = change.Value?.ToString();
        effortLevel.Level = value;

        ProjectSettingsChanged.InvokeAsync();
    }

    private void EffortDelete(EffortLevel effortLevel)
    {
        ProjectSettings.EffortLevels.Remove(effortLevel);

        ProjectSettingsChanged.InvokeAsync();
    }

    private void EffortAdd()
    {
        var lastEffort = ProjectSettings.EffortLevels.LastOrDefault();

        ProjectSettings.EffortLevels
            .Add(new EffortLevel
            {
                Effort = lastEffort.Effort * 2,
                Level = lastEffort.Level
            });

        ProjectSettingsChanged.InvokeAsync();
    }


    private void OverheadUpdateName(ProjectOverhead overhead, ChangeEventArgs change)
    {
        if (overhead == null || change == null)
            return;

        overhead.Name = change.Value?.ToString();

        ProjectSettingsChanged.InvokeAsync();
    }

    private void OverheadUpdateDescription(ProjectOverhead overhead, ChangeEventArgs change)
    {
        if (overhead == null || change == null)
            return;

        overhead.Description = change.Value?.ToString();

        ProjectSettingsChanged.InvokeAsync();
    }

    private void OverheadUpdateMultiplier(ProjectOverhead overhead, ChangeEventArgs change)
    {
        if (overhead == null || change == null)
            return;

        var value = change.Value?.ToString()?.ToDouble();
        overhead.Multiplier = value ?? 0;

        ProjectSettingsChanged.InvokeAsync();
    }
    private void OverheadDelete(ProjectOverhead overhead)
    {
        ProjectSettings.Overhead.Remove(overhead);

        ProjectSettingsChanged.InvokeAsync();
    }

    private void OverheadAdd()
    {
        ProjectSettings.Overhead.Add(new ProjectOverhead());

        ProjectSettingsChanged.InvokeAsync();
    }

}
