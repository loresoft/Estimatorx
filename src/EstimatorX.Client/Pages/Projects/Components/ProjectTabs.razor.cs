using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class ProjectTabs
{
    [Inject]
    public ProjectStore Store { get; set; }

    public Project Model => Store.Model;

}
