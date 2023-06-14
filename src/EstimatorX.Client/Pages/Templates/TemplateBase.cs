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

    public Project Model => TemplateStore.Model;

    protected override async Task OnInitializedAsync()
    {
        TemplateStore.OnChange += HandleModelChange;

        try
        {
            await TemplateStore.InitializedAsync();

            await TemplateStore.Load(Id, OrganizationId);
            if (TemplateStore.Model == null)
                Navigation.NavigateTo("/templates");

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

        InvokeAsync(() => StateHasChanged());
    }

    public virtual void Dispose()
    {
        TemplateStore.OnChange -= HandleModelChange;
    }
}
