
using Blazored.Modal.Services;

using EstimatorX.Client.Extensions;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class EpicContainer
{
    [CascadingParameter]
    public IModalService Modal { get; set; }

    private string ParentCollapse => $"epic-parent-{Project?.Id}";

    private string Identifier(string name) => $"epic-{name}-{Project?.Id}";

    private void EpicAdd()
    {
        Project.Epics.Add(new EpicEstimate { Name = "New Epic Estimate" });
        ProjectStore.NotifyStateChanged();
    }

    private async Task EpicReporder()
    {
        var result = await Modal.Reorder(Project.Epics);
        ProjectStore.NotifyStateChanged();
    }
}
