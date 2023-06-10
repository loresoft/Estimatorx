using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace EstimatorX.Client.Components;

public partial class AutoSave
{
    [CascadingParameter]
    private EditContext EditContext { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; }

    [Parameter]
    public EventCallback Save { get; set; }

    [Inject]
    private ILogger<AutoSave> Logger { get; set; }

    protected override void OnInitialized()
    {
        if (EditContext == null)
            throw new InvalidOperationException($"{nameof(AutoSave)} requires a cascading parameter of type {nameof(EditContext)}.");

        EditContext.OnFieldChanged += HandleFieldChange;
    }

    private void HandleFieldChange(object sender, FieldChangedEventArgs fieldChangedEventArgs)
    {
        Logger.LogDebug("Field {fieldName} changed", fieldChangedEventArgs.FieldIdentifier.FieldName);
        Save.InvokeAsync();
    }
}
