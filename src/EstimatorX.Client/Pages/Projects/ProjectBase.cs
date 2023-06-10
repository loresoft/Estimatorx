using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;
using EstimatorX.Shared.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects;

[Authorize]
public abstract class ProjectBase : ComponentBase, IDisposable
{
    [Parameter]
    public string Id { get; set; }

    [Parameter]
    public string OrganizationId { get; set; }


    [Inject]
    public NotificationService NotificationService { get; set; }

    [Inject]
    public ProjectStore Store { get; set; }

    [Inject]
    public NavigationManager Navigation { get; set; }

    [Inject]
    public IProjectCalculator ProjectCalculator { get; set; }

    [Inject]
    public IProjectBuilder ProjectBuilder { get; set; }

    public Project Model => Store.Model;

    protected override async Task OnInitializedAsync()
    {
        Store.OnChange += HandleModelChange;

        try
        {
            await Store.InitializedAsync();

            await Store.Load(Id, OrganizationId);
            if (Store.Model == null)
                Navigation.NavigateTo("/projects");

            ProjectBuilder.UpdateProject(Model);
            ProjectCalculator.UpdateProject(Model);
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
        }
    }

    private void HandleModelChange()
    {
        if (Model != null)
            ProjectCalculator.UpdateProject(Model);

        InvokeAsync(StateHasChanged);
    }

    public virtual void Dispose()
    {
        Store.OnChange -= HandleModelChange;
    }
}
