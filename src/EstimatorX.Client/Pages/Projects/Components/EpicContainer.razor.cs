using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class EpicContainer
{
    [Parameter]
    public List<EpicEstimate> Epics { get; set; }

    [Inject]
    public ProjectStore ProjectStore { get; set; }

    public Project Project => ProjectStore.Model;

    private string ParentCollapse => $"epic-parent-{Project?.Id}";

    private string Identifier(string name) => $"epic-{name}-{Project?.Id}";

    private void EpicAdd()
    {
        Epics.Add(new EpicEstimate { Name = "New Epic Estimate" });
        ProjectStore.NotifyStateChanged();
    }

    private void EpicReporder()
    {
        ProjectStore.NotifyStateChanged();
    }
}
