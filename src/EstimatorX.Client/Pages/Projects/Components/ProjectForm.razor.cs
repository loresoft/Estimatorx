using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class ProjectForm
{
    [Inject]
    public ProjectStore ProjectStore { get; set; }

    [Inject]
    public UserStore UserStore { get; set; }

    public Project Project => ProjectStore.Model;
}