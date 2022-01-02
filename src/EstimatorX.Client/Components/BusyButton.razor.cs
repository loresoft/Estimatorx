using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Components;

public partial class BusyButton
{
    [Parameter]
    public bool Busy { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public RenderFragment BusyTemplate { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> AdditionalAttributes { get; set; } = new Dictionary<string, object>();

    protected override void OnInitialized()
    {
        BusyTemplate ??= builder => builder.AddContent(0, "Busy...");
    }
}
