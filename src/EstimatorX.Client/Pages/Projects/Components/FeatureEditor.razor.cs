using System.Text.Json;

using EstimatorX.Client.Extensions;
using EstimatorX.Client.Services;
using EstimatorX.Client.Shared;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class FeatureEditor
{
    [Parameter]
    public string ParentCollapse { get; set; }

    [Parameter]
    public EpicEstimate Epic { get; set; }

    [Parameter]
    public FeatureEstimate Feature { get; set; }

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Inject]
    public ProjectStore ProjectStore { get; set; }

    public ProjectSettings ProjectSettings => ProjectStore.Model?.Settings;


    private string Identifier(string name) => $"feature-{name}-{Feature?.Id}";

    private double? OverheadEstimate(double multiplier, double? estimate)
    {
        var value = (estimate * multiplier) - estimate;
        return value.HasValue ? Math.Round(value.Value, 2) : null;
    }

    private double? OverheadAmmount(double multiplier, double? estimate)
    {
        var value = OverheadEstimate(multiplier, estimate);
        return value.HasValue ? value.Value * ProjectSettings.EstimateRate : null;
    }

    private async Task FeatureDelete()
    {
        var name = Feature.Name;
        if (!await JSRuntime.Confirm($"Are you sure you want to feature delete '{name}'?"))
            return;

        Epic.Features.Remove(Feature);

        ProjectStore.NotifyStateChanged();
    }

    private void FeatureDuplicate()
    {
        var json = JsonSerializer.Serialize(Feature);
        var clone = JsonSerializer.Deserialize<FeatureEstimate>(json);

        clone.Id = Guid.NewGuid().ToString("N");
        clone.Name += " - Copy";

        Epic.Features.Add(clone);

        ProjectStore.NotifyStateChanged();
    }
}
