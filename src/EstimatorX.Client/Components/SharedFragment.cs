using EstimatorX.Domain.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Components;

public static class SharedFragment
{
    /// <summary>Result template for user typeahead items.</summary>
    public static RenderFragment<UserReadModel> UserResult => item => builder =>
    {
        builder.OpenElement(0, "div");
        builder.AddContent(1, item.Name);
        builder.CloseElement(); // div

        builder.OpenElement(2, "div");
        builder.AddAttribute(3, "style", "font-size: .75rem; opacity: .75");
        builder.AddContent(4, item.Email);
        builder.CloseElement(); // div
    };

    public static RenderFragment EstimatedWeeksHeader => builder =>
    {
        // <abbr title="Estimated Weeks">E. Weeks</abbr>
        builder.OpenElement(0, "abbr");
        builder.AddAttribute(1, "title", "Estimated Weeks");
        builder.AddContent(2, "E. Weeks");
        builder.CloseElement(); // abbr
    };

    public static RenderFragment WeightedWeeksHeader => builder =>
    {
        // <abbr title="Weighted Weeks">W. Weeks</abbr>
        builder.OpenElement(0, "abbr");
        builder.AddAttribute(1, "title", "Weighted Weeks");
        builder.AddContent(2, "W. Weeks");
        builder.CloseElement(); // abbr
    };

    public static RenderFragment EstimatedCostHeader => builder =>
    {
        // <abbr title="Estimated Cost">E. Cost</abbr>
        builder.OpenElement(0, "abbr");
        builder.AddAttribute(1, "title", "Estimated Cost");
        builder.AddContent(2, "E. Cost");
        builder.CloseElement(); // abbr
    };

    public static RenderFragment WeightedCostHeader => builder =>
    {
        // <abbr title="Weighted Cost">W. Cost</abbr>
        builder.OpenElement(0, "abbr");
        builder.AddAttribute(1, "title", "Weighted Cost");
        builder.AddContent(2, "W. Cost");
        builder.CloseElement(); // abbr
    };

}
