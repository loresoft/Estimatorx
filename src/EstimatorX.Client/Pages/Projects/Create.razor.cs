using EstimatorX.Client.Repositories;
using EstimatorX.Client.Services;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects
{
    [Authorize]
    public partial class Create
    {
        [Inject]
        public NotificationService NotificationService { get; set; }

        [Inject]
        public ProjectStore ProjectStore { get; set; }

        [Inject]
        public NavigationManager Navigation { get; set; }


        public Project Project => ProjectStore.Model;
      

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ProjectStore.New();
        }

        protected async Task HandleSave()
        {
            try
            {
                var result = await ProjectStore.Save();

                NotificationService.ShowSuccess($"Project '{result.Name}' saved successfully");
                Navigation.NavigateTo($"/projects/{result.Id}/{result.OrganizationId}");
            }
            catch (Exception ex)
            {
                NotificationService.ShowError(ex);
            }
        }
    }
}
