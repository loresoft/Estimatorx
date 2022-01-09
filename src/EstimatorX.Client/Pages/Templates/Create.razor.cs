using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;
using EstimatorX.Shared.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Templates;

[Authorize]
public partial class Create
{
    [Inject]
    public NotificationService NotificationService { get; set; }

    [Inject]
    public TemplateStore Store { get; set; }

    [Inject]
    public NavigationManager Navigation { get; set; }

    [Inject]
    public IProjectCalculator ProjectCalculator { get; set; }

    [Inject]
    public IProjectBuilder ProjectBuilder { get; set; }


    public Template Model => Store.Model;


    protected override void OnInitialized()
    {
        base.OnInitialized();
        Store.New();
    }

    protected async Task HandleSave()
    {
        try
        {
            ProjectBuilder.UpdateProject(Model);
            ProjectCalculator.UpdateProject(Model);

            var result = await Store.Save();

            NotificationService.ShowSuccess($"Template '{result.Name}' saved successfully");
            Navigation.NavigateTo($"/templates/{result.Id}/{result.OrganizationId}");
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
        }
    }
}
