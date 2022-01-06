using EstimatorX.Client.Repositories;
using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using LoreSoft.Blazor.Controls;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects;

[Authorize]
public partial class Index
{
    [Inject]
    public NotificationService NotificationService { get; set; }

    [Inject]
    public ProjectRepository Repository { get; set; }

    [Inject]
    public UserStore UserStore { get; set; }


    private DataGrid<ProjectSummary> DataGrid { get; set; }

    private ICollection<ProjectSummary> Projects { get; set; } = new List<ProjectSummary>();

    private string SearchText { get; set; }

    private string SelectedOrganization { get; set; }

    private async ValueTask<DataResult<ProjectSummary>> LoadData(DataRequest request)
    {
        try
        {
            var sort = request.Sorts.FirstOrDefault();
            var query = new QueryRequest { Page = request.Page, PageSize = request.PageSize, Sort = sort?.Property, Descending = sort?.Descending, Search = SearchText, Organization = SelectedOrganization };
            var result = await Repository.Search(query);
            var response = new DataResult<ProjectSummary>(result.Total, result.Data);
            return response;
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
            var response = new DataResult<ProjectSummary>(0, Enumerable.Empty<ProjectSummary>());
            return response;
        }
    }

    private async Task HandleOrganizationChange(ChangeEventArgs e)
    {
        SelectedOrganization = e.Value?.ToString();
        await DataGrid.RefreshAsync(true);
    }

    private async Task HandleSearch()
    {
        await DataGrid.RefreshAsync(true);
    }
}
