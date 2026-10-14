using Arbiter.Components;
using Arbiter.Components.Abstracts;

using EstimatorX.Domain.Models;
using EstimatorX.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace EstimatorX.Client.Pages.Projects;

[Authorize]
public class ProjectBase : EditPageBase<long, ProjectReadModel, ProjectUpdateModel>
{
    [Inject]
    protected IProjectBuilder ProjectBuilder { get; set; } = default!;

    [Inject]
    protected IProjectCalculator ProjectCalculator { get; set; } = default!;


    protected ProjectSettings? ProjectSettings => Store.Model?.Settings;


    protected override string? GetRedirectLocation(RedirectReason reason, long id)
        => reason == RedirectReason.Created ? RouteLinks.Projects.Summary(id) : RouteLinks.Projects.List();

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

        // ensure the security key is set
        Model.SecurityKey ??= TokenService.GenerateCode();

        // save either the organization or the user, but not both
        if (Model.OrganizationId is null)
            Model.UserId = await GetUserId<long>();
        else
            Model.UserId = null;

        // ensure the project is updated with the latest calculations
        ProjectBuilder.UpdateProject(Model);
        ProjectCalculator.UpdateProject(Model);

        return await base.OnSavingAsync(cancellationToken);
    }

}
