using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace EstimatorX.Client.Pages.Organizations;

[Authorize]
public abstract class OrganizationBase : ComponentBase, IDisposable
{
    [Parameter]
    public string Id { get; set; }


    [Inject]
    public IJSRuntime JSRuntime { get; set; }

    [Inject]
    public NotificationService NotificationService { get; set; }

    [Inject]
    public OrganizationStore OrganizationStore { get; set; }

    [Inject]
    public NavigationManager Navigation { get; set; }


    public Organization Organization => OrganizationStore?.Model;

    public ICollection<OrganizationMember> MemberList => OrganizationStore?.Model?.Members;


    protected override async Task OnInitializedAsync()
    {
        OrganizationStore.OnChange += StateHasChanged;

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

    public void Dispose()
    {
        OrganizationStore.OnChange -= StateHasChanged;
    }

}
