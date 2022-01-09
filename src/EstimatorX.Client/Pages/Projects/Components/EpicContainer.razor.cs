
using Blazored.Modal.Services;

using EstimatorX.Client.Extensions;
using EstimatorX.Client.Repositories;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class EpicContainer<TStore, TRepository, TModel>
    where TStore : StoreEditBase<TModel, TRepository>
    where TRepository : RepositoryEditBase<TModel>
    where TModel : Project, IHaveIdentifier, new()
{
    [CascadingParameter]
    public IModalService Modal { get; set; }

    private string ParentCollapse => $"epic-parent-{Model?.Id}";

    private string Identifier(string name) => $"epic-{name}-{Model?.Id}";

    private void EpicAdd()
    {
        Model.Epics.Add(new EpicEstimate { Name = "New Epic Estimate" });
        Store.NotifyStateChanged();
    }

    private async Task EpicReorder()
    {
        var result = await Modal.Reorder(Model.Epics);
        Store.NotifyStateChanged();
    }
}
