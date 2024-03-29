using EstimatorX.Client.Repositories;
using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;
using EstimatorX.Shared.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects;

[Authorize]
public partial class Create
{
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

    [Inject]
    public TemplateRepository TemplateRespository { get; set; }

    public Project Model => Store.Model;

    public List<TemplateSummary> Templates { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Store.New();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        try
        {
            var request = new QueryRequest
            {
                Sort = nameof(Template.Name),
                PageSize = 1000,
            };

            var results = await TemplateRespository.Search(request);
            Templates = results.Data.ToList();
        }
        catch (Exception ex)
        {

            NotificationService.ShowError(ex);
        }
    }

    protected async Task HandleSave()
    {
        try
        {
            ProjectBuilder.UpdateProject(Model);
            ProjectCalculator.UpdateProject(Model);

            var result = await Store.Save();

            NotificationService.ShowSuccess($"Project '{result.Name}' saved successfully");
            Navigation.NavigateTo($"/projects/{result.Id}/{result.OrganizationId}");
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
        }
    }
}
