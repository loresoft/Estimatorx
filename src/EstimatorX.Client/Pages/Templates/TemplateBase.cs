using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;
using EstimatorX.Shared.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Templates;

[Authorize]
public abstract class TemplateBase : ComponentBase, IDisposable
{
    [Parameter]
    public string Id { get; set; }

    [Parameter]
    public string OrganizationId { get; set; }


    [Inject]
    public NotificationService NotificationService { get; set; }

    [Inject]
    public TemplateStore TemplateStore { get; set; }

    [Inject]
    public NavigationManager Navigation { get; set; }

    [Inject]
    public IProjectCalculator ProjectCalculator { get; set; }

    [Inject]
    public IProjectBuilder ProjectBuilder { get; set; }

    public Project Project => TemplateStore.Model;

    protected override async Task OnInitializedAsync()
    {
        TemplateStore.OnChange += HandleModelChange;

        try
        {
            await TemplateStore.Load(Id, OrganizationId);
            if (TemplateStore.Model == null)
                Navigation.NavigateTo("/templates");

            ProjectBuilder.UpdateProject(Project);
            ProjectCalculator.UpdateProject(Project);
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
        }
    }

    private void HandleModelChange()
    {
        if (Project != null)
            ProjectCalculator.UpdateProject(Project);

        InvokeAsync(() => StateHasChanged());
    }

    public virtual void Dispose()
    {
        TemplateStore.OnChange -= HandleModelChange;
    }
}
