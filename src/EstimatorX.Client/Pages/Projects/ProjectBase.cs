using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace EstimatorX.Client.Pages.Projects;

[Authorize]
public abstract class ProjectBase : ComponentBase, IDisposable
{
    [Parameter]
    public string Id { get; set; }

    [Parameter]
    public string OrganizationId { get; set; }


    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Inject]
    public NotificationService NotificationService { get; set; }

    [Inject]
    public ProjectStore ProjectStore { get; set; }

    [Inject]
    public NavigationManager Navigation { get; set; }

    public Project Project => ProjectStore.Model;

    protected override async Task OnInitializedAsync()
    {
        ProjectStore.OnChange += HandleModelChange;

        try
        {
            await ProjectStore.Load(Id, OrganizationId);
            if (ProjectStore.Model == null)
                Navigation.NavigateTo("/projects");
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
        }
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
