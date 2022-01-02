using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class FeatureEditor
{
    [Parameter]
    public string ParentCollapse { get; set; }

    [Parameter]
    public FeatureEstimate Feature { get; set; }

    [Inject]
    public ProjectStore ProjectStore { get; set; }

    public ProjectSettings ProjectSettings => ProjectStore.Model.Settings;


    private Guid Id = Guid.NewGuid();

    private string Identifier(string name) => $"feature-{name}-{Id}";

    private double? OverheadEstimate(double multiplier, double? estimate)
    {
        var value = (estimate * multiplier) - estimate;
        return value.HasValue ? Math.Round(value.Value, 2) : null;
    }

    private double? OverheadAmmount(double multiplier, double? estimate)
    {
        var value = OverheadEstimate(multiplier, estimate);
        return value.HasValue ? value.Value * ProjectSettings.EstimateRate : null;
    }
}
