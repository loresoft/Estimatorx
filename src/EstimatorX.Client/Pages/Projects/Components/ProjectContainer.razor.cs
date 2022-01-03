using Blazored.Modal;
using Blazored.Modal.Services;

using EstimatorX.Client.Components;
using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;
using EstimatorX.Shared.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class ProjectContainer : IDisposable
{
    [Parameter]
    public string Id { get; set; }

    [Parameter]
    public string OrganizationId { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public string BodyClass { get; set; }


    [CascadingParameter]
    public IModalService Modal { get; set; }


    [Inject]
    public NotificationService NotificationService { get; set; }

    [Inject]
    public ProjectStore ProjectStore { get; set; }

    [Inject]
    public IProjectCalculator ProjectCalculator { get; set; }

    [Inject]
    public IProjectBuilder ProjectBuilder { get; set; }


    [Inject]
    public NavigationManager Navigation { get; set; }

    public Project Project => ProjectStore.Model;

    private EditContext EditContext { get; set; }

    protected override async Task OnInitializedAsync()
    {
        ProjectStore.OnChange += HandleModelChange;

        try
        {
            await ProjectStore.Load(Id, OrganizationId);
            if (ProjectStore.Model == null)
                Navigation.NavigateTo("/projects");

            EditContext = new EditContext(ProjectStore.Model);
            EditContext.OnFieldChanged += HandleFormChange;

            ProjectBuilder.UpdateProject(Project);
            ProjectCalculator.UpdateProject(Project);
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
            var name = Project.Name;

            var parameters = new ModalParameters();
            parameters.Add(nameof(ConfirmDelete.Message), $"Are you sure you want to delete project '{name}'?");

            var messageForm = Modal.Show<ConfirmDelete>("Confirm Delete", parameters);
            var result = await messageForm.Result;

            if (result.Cancelled)
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

    private void HandleModelChange()
    {
        ProjectCalculator.UpdateProject(Project);

        InvokeAsync(() => StateHasChanged());
    }

    private void HandleFormChange(object sender, FieldChangedEventArgs args)
    {
        ProjectStore.NotifyStateChanged();
    }

    public void Dispose()
    {
        ProjectStore.OnChange -= HandleModelChange;
        EditContext.OnFieldChanged -= HandleFormChange;
    }
}
