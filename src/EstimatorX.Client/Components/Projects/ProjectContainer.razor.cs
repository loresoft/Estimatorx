using EstimatorX.Client.Extensions;
using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace EstimatorX.Client.Components.Projects;

public partial class ProjectContainer
{
    [Parameter]
    public string Id { get; set; }

    [Parameter]
    public string OrganizationId { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }


    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Inject]
    public NotificationService NotificationService { get; set; }

    [Inject]
    public ProjectStore ProjectStore { get; set; }

    [Inject]
    public NavigationManager Navigation { get; set; }


    protected override async Task OnInitializedAsync()
    {
        ProjectStore.OnChange += StateHasChanged;

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

    protected virtual async Task HandleSave()
    {
        try
        {
            await ProjectStore.Save(Id, OrganizationId);

            var model = ProjectStore.Model;

            NotificationService.ShowSuccess($"Project '{model.Name}' saved successfully");

            // route if org changed
            if (model.OrganizationId != OrganizationId)
                Navigation.NavigateTo($"/projects/{model.Id}/{model.OrganizationId}");
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
        }
    }

    protected virtual async Task HandleDelete()
    {
        try
        {
            var name = ProjectStore.Model.Name;
            if (!await JSRuntime.Confirm($"Are you sure you want to delete '{name}'?"))
                return;
            await ProjectStore.Delete(Id, OrganizationId);
            NotificationService.ShowSuccess($"Project '{name}' deleted successfully");
            Navigation.NavigateTo("/projects");
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
        }
    }


    public void Dispose()
    {
        ProjectStore.OnChange -= StateHasChanged;
    }
}
