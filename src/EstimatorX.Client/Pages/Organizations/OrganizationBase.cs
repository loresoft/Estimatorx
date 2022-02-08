using Blazored.Modal.Services;

using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Organizations;

[Authorize]
public abstract class OrganizationBase : ComponentBase, IDisposable
{
    [Parameter]
    public string Id { get; set; }


    [CascadingParameter]
    public IModalService Modal { get; set; }


    [Inject]
    public NotificationService NotificationService { get; set; }

    [Inject]
    public OrganizationStore OrganizationStore { get; set; }

    [Inject]
    public NavigationManager Navigation { get; set; }

    [Inject]
    public UserStore UserStore { get; set; }


    public Organization Organization => OrganizationStore?.Model;

    public ICollection<OrganizationMember> MemberList => OrganizationStore?.Model?.Members;

    protected bool IsOwner() => Organization?.Members?.Any(m => m.Id == UserStore?.Model?.Id && m.IsOwner) == true;

    protected override async Task OnInitializedAsync()
    {
        OrganizationStore.OnChange += HandleModelChange;

        try
        {
            await OrganizationStore.Load(Id);
            if (OrganizationStore.Model == null)
                Navigation.NavigateTo("/organizations");
        }
        catch (Exception ex)
        {
            NotificationService.ShowError(ex);
        }
    }

    private void HandleModelChange()
    {
        InvokeAsync(() => StateHasChanged());
    }


    public void Dispose()
    {
        OrganizationStore.OnChange -= HandleModelChange;
    }

}
