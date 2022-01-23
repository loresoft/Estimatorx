using EstimatorX.Client.Extensions;
using EstimatorX.Client.Repositories;
using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using LoreSoft.Blazor.Controls;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Organizations;

[Authorize]
public partial class Members : OrganizationBase
{
    [Inject]
    public UserRepository UserRepository { get; set; }

    [Inject]
    public UserStore UserStore { get; set; }


    private DataGrid<OrganizationMember> DataGrid { get; set; }

    private UserSummary SelectedUser { get; set; }


    protected bool IsSelf(string id)
    {
        return UserStore.Model.Id == id;
    }


    protected async Task HandleToggleOwner(string id)
    {
        var member = Organization.Members.FirstOrDefault(m => m.Id == id);
        if (member == null)
            return;

        member.IsOwner = !member.IsOwner;

        OrganizationStore.NotifyStateChanged();
        await DataGrid.RefreshAsync();
        await InvokeAsync(() => StateHasChanged());
    }

    protected async Task HandleRemoveUser(string id)
    {
        var member = Organization.Members.FirstOrDefault(m => m.Id == id);
        if (member == null)
            return;

        var name = member.Name;

        if (!await Modal.ConfirmDelete($"Are you sure you want to remove '{name}'?"))
            return;

        Organization.Members.Remove(member);

        OrganizationStore.NotifyStateChanged();
        await DataGrid.RefreshAsync();
        await InvokeAsync(() => StateHasChanged());
    }

    protected async Task HandleAddUser()
    {
        if (SelectedUser == null)
            return;

        // prevent duplicate
        if (Organization.Members.Any(m => m.Id == SelectedUser.Id))
        {
            NotificationService.ShowError($"User '{SelectedUser.Name}' already a member of '{Organization.Name}'");
            return;
        }

        Organization.Members.Add(new OrganizationMember
        {
            Id = SelectedUser.Id,
            Name = SelectedUser.Name,
            Email = SelectedUser.Email
        });

        OrganizationStore.NotifyStateChanged();
        await DataGrid.RefreshAsync();
        await InvokeAsync(() => StateHasChanged());

        SelectedUser = null;
    }

    protected async Task<IEnumerable<UserSummary>> SearchUsers(string searchText)
    {
        try
        {
            var query = new QueryRequest { PageSize = 100, Search = searchText, Sort = nameof(User.Name) };
            var result = await UserRepository.Search(query);
            return result.Data;
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
            return Enumerable.Empty<UserSummary>();
        }
    }
}
