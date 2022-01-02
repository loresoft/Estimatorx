using EstimatorX.Shared.Services;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects;

public partial class Settings : ProjectBase
{
    [Inject]
    public IProjectBuilder ProjectBuilder { get; set; }

    protected override void OnParametersSet()
    {
        ProjectBuilder.UpdateSettings(ProjectStore.Model.Settings);
        ProjectStore.NotifyStateChanged();
    }

    private void HandleModelChange()
    {
        ProjectStore.NotifyStateChanged();
        InvokeAsync(() => StateHasChanged());
    }


}
