using EstimatorX.Client.Repositories;
using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Invite;

public partial class Index
{
    [Parameter]
    public string SecurityKey { get; set; }

    [Inject]
    public NavigationManager Navigation { get; set; }

    [Inject]
    public NotificationService NotificationService { get; set; }

    [Inject]
    public InviteRepository InviteRepository { get; set; }

    [Inject]
    public UserStore UserStore { get; set; }

    private bool IsBusy { get; set; }

    private EstimatorX.Shared.Models.Invite Invite { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            IsBusy = true;
            Invite = await InviteRepository.LoadByKey(SecurityKey);
        }
        catch (Exception ex)
        {

            NotificationService.ShowError(ex);
            Navigation.NavigateTo("/");
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected async Task HandleAccept()
    {
        try
        {
            IsBusy = true;

            await InviteRepository.Redeem(SecurityKey);

            NotificationService.ShowSuccess($"Invite to '{Invite.OrganizationName}' accepted successfully");
            Navigation.NavigateTo("/Account/Profile");
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }
}
