using EstimatorX.Client.Extensions;
using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
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

    protected override void OnInitialized()
    {
        ProjectStore.OnChange += StateHasChanged;
    }

    public void Dispose()
    {
        ProjectStore.OnChange -= StateHasChanged;
    }
}
