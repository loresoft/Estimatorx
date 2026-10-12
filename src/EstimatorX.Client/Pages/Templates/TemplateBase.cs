using Arbiter.Components;
using Arbiter.Components.Abstracts;

using EstimatorX.Domain.Models;
using EstimatorX.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace EstimatorX.Client.Pages.Templates;

[Authorize]
public class TemplateBase : EditPageBase<long, TemplateReadModel, TemplateUpdateModel>
{
    [Inject]
    protected IProjectBuilder ProjectBuilder { get; set; } = default!;

    [Inject]
    protected IProjectCalculator ProjectCalculator { get; set; } = default!;


    protected ProjectSettings? ProjectSettings => Store.Model?.Settings;


    protected override string? GetRedirectLocation(RedirectReason reason, long id)
        => reason == RedirectReason.Created ? RouteLinks.Templates.General(id) : RouteLinks.Templates.List();

    protected override void HandleModelChange(object? sender, EventArgs e)
    {
        if (Model != null)
            ProjectCalculator.UpdateProject(Model);

        base.HandleModelChange(sender, e);
    }

    protected override void HandleFormChange(object? sender, FieldChangedEventArgs args)
    {
        if (Model != null)
            ProjectCalculator.UpdateProject(Model);

        base.HandleFormChange(sender, args);

        StateHasChanged();
    }

    protected override async Task<bool> OnSavingAsync(CancellationToken cancellationToken)
    {
        if (Model == null)
            return false;

        // save either the organization or the user, but not both
        if (Model.OrganizationId is null)
            Model.UserId = await GetUserId<long>();
        else
            Model.UserId = null;

        ProjectBuilder.UpdateProject(Model);
        ProjectCalculator.UpdateProject(Model);

        return await base.OnSavingAsync(cancellationToken);
    }
}
