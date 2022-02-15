using Blazored.Modal;
using Blazored.Modal.Services;

using EstimatorX.Client.Repositories;
using EstimatorX.Client.Services;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Organizations.Components;

public partial class OrganizationInviteModal
{
    [Parameter]
    public Organization Organization { get; set; }

    private bool IsBusy { get; set; }

    private EstimatorX.Shared.Models.Invite Invite { get; set; } = new();

    [CascadingParameter]
    private BlazoredModalInstance Modal { get; set; }

    [Inject]
    public NotificationService NotificationService { get; set; }

    [Inject]
    public InviteRepository InviteRepository { get; set; }

    private async Task HandleSave()
    {
        try
        {
            IsBusy = true;

            Invite.OrganizationId = Organization.Id;
            Invite.OrganizationName = Organization.Name;

            var invite = await InviteRepository.Create(Invite);

            NotificationService.ShowSuccess($"Invite for '{invite.Name}' sent successfully");

            await Modal.CloseAsync(ModalResult.Ok(true));
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

    private async Task Cancel() => await Modal.CancelAsync();
}
