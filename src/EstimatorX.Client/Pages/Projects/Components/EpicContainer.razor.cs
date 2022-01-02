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

    private Guid Id = Guid.NewGuid();

    private string ParentCollapse => $"epic-parent-{Id}";

    private string Identifier(string name) => $"epic-{name}-{Id}";

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
