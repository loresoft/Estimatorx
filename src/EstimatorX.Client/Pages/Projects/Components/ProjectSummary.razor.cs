using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class ProjectSummary
{
    [Inject]
    public ProjectStore ProjectStore { get; set; }

    public Project Project => ProjectStore.Model;
}
