using Blazored.Modal.Services;
using Blazored.Modal;

using EstimatorX.Client.Components;
using EstimatorX.Client.Extensions;
using EstimatorX.Client.Repositories;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;
using LoreSoft.Blazor.Controls;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using EstimatorX.Client.Pages.Organizations.Components;

namespace EstimatorX.Client.Pages.Organizations;

[Authorize]
public partial class Invites : OrganizationBase
{
    [Inject]
    public InviteRepository InviteRepository { get; set; }

    [Inject]
    public UserStore UserStore { get; set; }


    private DataGrid<InviteSummary> DataGrid { get; set; }

    private string SearchText { get; set; }


    private async ValueTask<DataResult<InviteSummary>> LoadData(DataRequest request)
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
                Search = SearchText,
                Organization = Organization?.Id
            };
            var result = await InviteRepository.Search(query);

            return new DataResult<InviteSummary>(result.Total, result.Data);
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
            return new DataResult<InviteSummary>(0, Enumerable.Empty<InviteSummary>());
        }
    }

    private async Task HandleSendInvite(InviteSummary invite)
    {
        try
        {
            var name = invite.Name;

            var parameters = new ModalParameters();
            parameters.Add(nameof(ConfirmModal.Message), $"Are you sure you want to send invite email to '{name}'?");
            parameters.Add(nameof(ConfirmModal.ActionClass), "btn-primary");
            parameters.Add(nameof(ConfirmModal.ActionName), "Send");

            var messageForm = Modal.Show<ConfirmModal>("Confirm Send", parameters);
            var result = await messageForm.Result;

            if (result.Cancelled)
                return;


            await InviteRepository.Send(invite.Id, invite.OrganizationId);
            NotificationService.ShowSuccess($"Invite to '{name}' sent successfully");

            await DataGrid.RefreshAsync();
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
        }
    }

    private async Task HandleDeleteInvite(InviteSummary invite)
    {
        try
        {
            var name = invite.Name;

            if (!await Modal.ConfirmDelete($"Are you sure you want to delete invite for '{name}'?"))
                return;


            await InviteRepository.Delete(invite.Id, invite.OrganizationId);
            NotificationService.ShowSuccess($"Invite '{name}' deleted successfully");

            await DataGrid.RefreshAsync();
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
        }
    }

    private async Task HandleNewInvite()
    {
        try
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(OrganizationInviteModal.Organization), Organization);

            var messageForm = Modal.Show<OrganizationInviteModal>("Send Invite", parameters);
            var result = await messageForm.Result;

            if (result.Cancelled)
                return;

            await DataGrid.RefreshAsync();
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
        }

    }
}
