using EstimatorX.Client.Stores;

using LoreSoft.Blazor.Controls.Utilities;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Components;

public partial class Accordion : IDisposable
{
    [Parameter]
    public RenderFragment Header { get; set; }

    [Parameter]
    public RenderFragment Body { get; set; }

    [Parameter]
    public bool Collapsed { get; set; } = true;

    [Parameter]
    public string ParentGroup { get; set; }

    [Inject]
    private AccordionStore AccordionStore { get; set; }

    private Guid Id = Guid.NewGuid();

    private bool Shown => !Collapsed;

    private string Identifier(string name) => $"accordion-{name}-{Id:N}";

    private string ButtonClass => new CssBuilder("accordion-button").AddClass("collapsed", Collapsed).ToString();

    private string BodyClass => new CssBuilder("accordion-collapse collapse").AddClass("show", Shown).ToString();

    private void ToggleCollapsed()
    {
        Collapsed = !Collapsed;
        AccordionStore.NotifyStateChanged(ParentGroup, Id, Collapsed);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        AccordionStore.OnChange += OnAccordionChange;
    }

    private void OnAccordionChange(string parentGroup, Guid accordionId, bool collapsed)
    {
        if (parentGroup != ParentGroup)
            return;

        if (accordionId == Id)
            return;

        Collapsed = true;
        StateHasChanged();
    }

    public void Dispose()
    {
        AccordionStore.OnChange -= OnAccordionChange;
    }
}
