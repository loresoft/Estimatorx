using System.Text.Json;

using Blazored.Modal;
using Blazored.Modal.Services;

using EstimatorX.Client.Components;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class FeatureEditor
{
    [Parameter]
    public string ParentCollapse { get; set; }

    [Parameter]
    public EpicEstimate Epic { get; set; }

    [Parameter]
    public FeatureEstimate Feature { get; set; }

    [CascadingParameter]
    public IModalService Modal { get; set; }

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

        var parameters = new ModalParameters();
        parameters.Add(nameof(ConfirmDelete.Message), $"Are you sure you want to delete feature '{name}'?");

        var messageForm = Modal.Show<ConfirmDelete>("Confirm Delete", parameters);
        var result = await messageForm.Result;

        if (result.Cancelled)
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
