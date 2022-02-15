using EstimatorX.Client.Repositories;
using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace EstimatorX.Client.Pages.Account;

[Authorize]
public partial class Profile
{
    [Inject]
    public NavigationManager Navigation { get; set; }

    [Inject]
    public NotificationService NotificationService { get; set; }

    [Inject]
    public UserRepository UserRepository { get; set; }

    [Inject]
    public UserStore UserStore { get; set; }


    private User UserModel { get; set; }

    private bool IsBusy { get; set; }

    private int OriginalHash { get; set; }

    private bool IsDiry => OriginalHash != UserModel.GetHashCode();

    protected override void OnInitialized()
    {
        base.OnInitialized();
        UserModel = UserStore.Model;
        OriginalHash = UserModel.GetHashCode();
    }

    protected async Task HandleSave()
    {
        try
        {
            IsBusy = true;
            UserModel = await UserRepository.Save(UserModel, UserModel.Id);

            UserStore.Set(UserModel);

            OriginalHash = UserModel.GetHashCode();

            NotificationService.ShowSuccess($"Profile '{UserModel.Name}' saved successfully");
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
