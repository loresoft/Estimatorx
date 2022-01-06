using System.Reflection;

using AutoMapper;

using Blazored.Modal.Services;

using EstimatorX.Client.Extensions;
using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;
using EstimatorX.Shared.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

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

    [Inject]
    public IMapper Mapper { get; set; }


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

    private async Task HandleSave()
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

    private async Task HandleDelete()
    {
        try
        {
            var name = Project.Name;

            if (!await Modal.ConfirmDelete($"Are you sure you want to delete project '{name}'?"))
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

    private async Task HandleDuplicate()
    {
        try
        {
            var name = Project.Name;

            var clone = Mapper.Map<Project>(Project);
            clone.Name += " - Copy";

            var result = await ProjectStore.Repository.Create(clone);

            NotificationService.ShowSuccess($"Project '{name}' duplicated successfully");

            Navigation.NavigateTo($"/projects");
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
        }
    }

    private async Task HandleMakeTemplate()
    {

    }

    private void HandleModelChange()
    {
        if (Project != null)
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
