
using Blazored.Modal.Services;

using EstimatorX.Client.Extensions;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class FeatureContainer
{
    [Parameter]
    public EpicEstimate Epic { get; set; }

    [CascadingParameter]
    public IModalService Modal { get; set; }


    private string ParentCollapse => $"feature-parent-{Epic?.Id}";

    private string Identifier(string name) => $"feature-{name}-{Epic?.Id}";

    private void FeatureAdd()
    {
        Epic.Features.Add(new FeatureEstimate { Name = "New Feature Estimate" });
        ProjectStore.NotifyStateChanged();
    }

    private async Task FeatureReporder()
    {
        var result = await Modal.Reorder(Epic.Features);
        ProjectStore.NotifyStateChanged();
    }

    private async Task EpicDelete()
    {
        var name = Epic.Name;

        if (!await Modal.ConfirmDelete($"Are you sure you want to delete epic '{name}'?"))
            return;


        Project.Epics.Remove(Epic);

        ProjectStore.NotifyStateChanged();
    }

    private void EpicDuplicate()
    {
        var clone = Epic.Clone();

        clone.Id = Guid.NewGuid().ToString("N");
        clone.Name += " - Copy";

        Project.Epics.Add(clone);

        ProjectStore.NotifyStateChanged();
    }
}
