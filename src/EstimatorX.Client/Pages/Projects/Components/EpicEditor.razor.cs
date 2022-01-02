using EstimatorX.Client.Stores;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class EpicEditor
{
    [Parameter]
    public string ParentCollapse { get; set; }

    [Parameter]
    public EpicEstimate Epic { get; set; }

    [Inject]
    public ProjectStore ProjectStore { get; set; }

    private Guid Id = Guid.NewGuid();

    private string Identifier(string name) => $"epic-{name}-{Id}";
}
