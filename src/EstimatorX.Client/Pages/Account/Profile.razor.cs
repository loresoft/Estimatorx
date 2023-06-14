using System.Text.Json;

using EstimatorX.Client.Repositories;
using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using Json.Patch;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

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

    [Inject]
    public JsonSerializerOptions JsonSerializerOptions { get; set; }

    private User UserModel { get; set; }

    private User Original { get; set; }

    private bool IsBusy { get; set; }

    private int OriginalHash { get; set; }

    private bool IsDiry => OriginalHash != UserModel.GetHashCode();

    protected override void OnInitialized()
    {
        base.OnInitialized();
        UserModel = UserStore.Model;
        OriginalHash = UserModel.GetHashCode();
        Original = UserModel.JsonClone(JsonSerializerOptions);
    }

    protected async Task HandleSave()
    {
        try
        {
            IsBusy = true;

            var jsonPatch = Original.CreatePatch(UserModel, JsonSerializerOptions);

            UserModel = await UserRepository.Patch(jsonPatch, UserModel.Id);

            UserStore.Set(UserModel);

            OriginalHash = UserModel.GetHashCode();
            Original = UserModel.JsonClone(JsonSerializerOptions);

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
