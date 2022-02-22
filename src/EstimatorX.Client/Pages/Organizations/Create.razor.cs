using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace EstimatorX.Client.Pages.Organizations;

[Authorize]
public partial class Create : IDisposable
{
    [Inject]
    public NotificationService NotificationService { get; set; }

    [Inject]
    public OrganizationStore OrganizationStore { get; set; }

    [Inject]
    public UserStore UserStore { get; set; }

    [Inject]
    public NavigationManager Navigation { get; set; }


    public Organization Organization => OrganizationStore.Model;

    private EditContext EditContext { get; set; }


    protected override void OnInitialized()
    {
        OrganizationStore.OnChange += HandleModelChange;

        OrganizationStore.New();

        var member = new OrganizationMember
        {
            Id = UserStore.Model.Id,
            Name = UserStore.Model.Name,
            Email = UserStore.Model.Email,
            IsOwner = true
        };

        Organization.Members.Add(member);

        EditContext = new EditContext(OrganizationStore.Model);
        EditContext.OnFieldChanged += HandleFormChange;
    }

    protected async Task HandleSave()
    {
        try
        {
            var result = await OrganizationStore.Save();

            NotificationService.ShowSuccess($"Organization '{result.Name}' saved successfully");
            Navigation.NavigateTo($"/organizations/{result.Id}");
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

    private void HandleFormChange(object sender, FieldChangedEventArgs args)
    {
        OrganizationStore.NotifyStateChanged();
    }

    public void Dispose()
    {
        OrganizationStore.OnChange -= HandleModelChange;
        EditContext.OnFieldChanged -= HandleFormChange;
    }

}
