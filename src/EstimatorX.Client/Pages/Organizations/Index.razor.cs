
using EstimatorX.Client.Repositories;
using EstimatorX.Client.Services;
using EstimatorX.Shared.Models;

using LoreSoft.Blazor.Controls;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Organizations;

[Authorize]
public partial class Index
{
    [Inject]
    public OrganizationRepository Repository { get; set; }

    [Inject]
    public NotificationService NotificationService { get; set; }


    private DataGrid<OrganizationSummary> DataGrid { get; set; }

    private string SearchText { get; set; }

    private async ValueTask<DataResult<OrganizationSummary>> LoadData(DataRequest request)
    {
        try
        {
            var sort = request.Sorts.FirstOrDefault();
            var query = new QueryRequest
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Sort = sort?.Property,
                Descending = sort?.Descending,
                Search = SearchText
            };
            var result = await Repository.Search(query);

            return new DataResult<OrganizationSummary>(result.Total, result.Data);
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
            return new DataResult<OrganizationSummary>(0, Enumerable.Empty<OrganizationSummary>());
        }
    }

    private async Task HandleSearch()
    {
        await DataGrid.RefreshAsync(true);
    }
}
