using System.Text.Json;

using EstimatorX.Client.Extensions;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class FeatureContainer
{
    [Parameter]
    public EpicEstimate Epic { get; set; }

    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Inject]
    public ProjectStore ProjectStore { get; set; }

    public Project Project => ProjectStore.Model;

    private string ParentCollapse => $"feature-parent-{Epic?.Id}";

    private string Identifier(string name) => $"feature-{name}-{Epic?.Id}";

    private void FeatureAdd()
    {
        Epic.Features.Add(new FeatureEstimate { Name = "New Feature Estimate" });
        ProjectStore.NotifyStateChanged();
    }

    private void FeatureReporder()
    {
        ProjectStore.NotifyStateChanged();
    }

    private async Task EpicDelete()
    {
        var name = Epic.Name;
        if (!await JSRuntime.Confirm($"Are you sure you want to delete epic '{name}'?"))
            return;

        Project.Epics.Remove(Epic);

        ProjectStore.NotifyStateChanged();
    }

    private void EpicDuplicate()
    {
        var json = JsonSerializer.Serialize(Epic);
        var clone = JsonSerializer.Deserialize<EpicEstimate>(json);

        clone.Id = Guid.NewGuid().ToString("N");
        clone.Name += " - Copy";

        Project.Epics.Add(clone);

        ProjectStore.NotifyStateChanged();
    }
}
