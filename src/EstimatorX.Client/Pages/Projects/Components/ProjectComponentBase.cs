using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public abstract class ProjectComponentBase : ComponentBase, IDisposable
{
    [Inject]
    public ProjectStore ProjectStore { get; set; }

    public Project Project => ProjectStore.Model;

    protected override void OnInitialized()
    {
        ProjectStore.OnChange += HandleModelChange;
    }

    private void HandleModelChange()
    {
        InvokeAsync(() => StateHasChanged());
    }

    public virtual void Dispose()
    {
        ProjectStore.OnChange -= HandleModelChange;
    }

}
