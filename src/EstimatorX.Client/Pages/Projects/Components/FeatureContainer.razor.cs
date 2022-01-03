using System.Text.Json;

using Blazored.Modal;
using Blazored.Modal.Services;

using EstimatorX.Client.Components;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class FeatureContainer
{
    [Parameter]
    public EpicEstimate Epic { get; set; }

    [CascadingParameter]
    public IModalService Modal { get; set; }

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

        var parameters = new ModalParameters();
        parameters.Add(nameof(ConfirmDelete.Message), $"Are you sure you want to delete epic '{name}'?");

        var messageForm = Modal.Show<ConfirmDelete>("Confirm Delete", parameters);
        var result = await messageForm.Result;

        if (result.Cancelled)
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
