using EstimatorX.Client.Stores;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class ProjectForm
{
    [Inject]
    public UserStore UserStore { get; set; }
}
