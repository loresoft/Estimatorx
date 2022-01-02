using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class FeatureContainer
{
    [Parameter]
    public List<FeatureEstimate> Features { get; set; }

    [Inject]
    public ProjectStore ProjectStore { get; set; }

    private Guid Id = Guid.NewGuid();

    private string ParentCollapse => $"feature-parent-{Id}";

    private string Identifier(string name) => $"feature-{name}-{Id}";

    private void FeatureAdd()
    {
        Features.Add(new FeatureEstimate { Name = "New Feature Estimate" });
        ProjectStore.NotifyStateChanged();
    }

    private void FeatureReporder()
    {
        ProjectStore.NotifyStateChanged();
    }
}
