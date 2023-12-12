using System.Text.Json;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace EstimatorX.Client.Components;

public class JsonDisplay : ComponentBase
{
    private int _sequence = 0;

    [Parameter]
    [EditorRequired]
    public string Json { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> Attributes { get; set; }


    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var document = JsonDocument.Parse(Json);

        AppendValue(builder, document.RootElement);
    }

    protected int Next() => _sequence++;

    private void AppendValue(RenderTreeBuilder builder, JsonElement jsonElement)
    {
        switch (jsonElement.ValueKind)
        {
            case JsonValueKind.Object:
                AppendObject(builder, jsonElement);
                break;
            case JsonValueKind.Array:
                foreach (var arrayElement in jsonElement.EnumerateArray())
                    AppendValue(builder, arrayElement);

                break;
            case JsonValueKind.String:
            case JsonValueKind.Number:
            case JsonValueKind.True:
            case JsonValueKind.False:
                builder.AddContent(Next(), jsonElement.ToString());
                break;
            case JsonValueKind.Undefined:
            case JsonValueKind.Null:
            default:
                builder.AddContent(Next(), string.Empty);
                break;
        }
    }

    private void AppendObject(RenderTreeBuilder builder, JsonElement jsonElement)
    {
        builder.OpenElement(Next(), "table");
        builder.AddAttribute(Next(), "class", "json-object");
        builder.AddMultipleAttributes(Next(), Attributes);

        foreach (var jsonProperty in jsonElement.EnumerateObject())
        {
            builder.OpenElement(Next(), "tr");

            builder.OpenElement(Next(), "th");
            builder.AddAttribute(Next(), "class", "json-name");
            builder.AddContent(Next(), jsonProperty.Name);
            builder.CloseElement(); // th

            builder.OpenElement(Next(), "td");
            builder.AddAttribute(Next(), "class", "json-value");

            AppendValue(builder, jsonProperty.Value);

            builder.CloseElement(); // td

            builder.CloseElement(); // tr
        }

        builder.CloseElement(); // table
    }
}
