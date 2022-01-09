
using AutoMapper;

using Blazored.Modal.Services;

using EstimatorX.Client.Extensions;
using EstimatorX.Client.Repositories;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class FeatureContainer<TStore, TRepository, TModel>
    where TStore : StoreEditBase<TModel, TRepository>
    where TRepository : RepositoryEditBase<TModel>
    where TModel : Project, IHaveIdentifier, new()
{
    [Parameter]
    public EpicEstimate Epic { get; set; }

    [CascadingParameter]
    public IModalService Modal { get; set; }

    [Inject]
    public IMapper Mapper { get; set; }

    private string ParentCollapse => $"feature-parent-{Epic?.Id}";

    private string Identifier(string name) => $"feature-{name}-{Epic?.Id}";

    private void FeatureAdd()
    {
        Epic.Features.Add(new FeatureEstimate { Name = "New Feature Estimate" });
        Store.NotifyStateChanged();
    }

    private async Task FeatureReporder()
    {
        var result = await Modal.Reorder(Epic.Features);
        Store.NotifyStateChanged();
    }

    private async Task EpicDelete()
    {
        var name = Epic.Name;

        if (!await Modal.ConfirmDelete($"Are you sure you want to delete epic '{name}'?"))
            return;


        Model.Epics.Remove(Epic);

        Store.NotifyStateChanged();
    }

    private void EpicDuplicate()
    {
        var clone = Mapper.Map<EpicEstimate>(Epic);

        clone.Id = Guid.NewGuid().ToString("N");
        clone.Name += " - Copy";

        Model.Epics.Add(clone);

        Store.NotifyStateChanged();
    }
}
