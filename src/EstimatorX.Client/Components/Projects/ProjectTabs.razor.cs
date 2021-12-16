using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Components.Projects;

public partial class ProjectTabs
{
    [Inject]
    public ProjectStore ProjectStore { get; set; }

    public Project Project => ProjectStore.Model;

}
