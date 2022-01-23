using Blazored.Modal;
using Blazored.Modal.Services;

using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class FeatureMoveModal
{
    [CascadingParameter]
    BlazoredModalInstance Modal { get; set; }

    [Parameter]
    public Project Project { get; set; }

    [Parameter]
    public FeatureEstimate Feature { get; set; }

    private string EpicId { get; set; }

    private async Task Close()
    {
        // remove from current
        var oldEpic = Project.Epics
             .Where(e => e.Features.Any(f => f == Feature))
             .FirstOrDefault();

        if (oldEpic != null)
            oldEpic.Features.Remove(Feature);

        // add to new
        var newEpic = Project.Epics
            .Where(e => e.Id == EpicId)
            .FirstOrDefault();

        if (newEpic != null)
            newEpic.Features.Add(Feature);

        await Modal.CloseAsync(ModalResult.Ok(true));
    }

    private async Task Cancel() => await Modal.CancelAsync();

    protected override void OnInitialized()
    {
        base.OnInitialized();

        EpicId = Project.Epics
            .Where(e => e.Features.Any(f => f == Feature))
            .Select(e => e.Id)
            .FirstOrDefault();
    }
}
