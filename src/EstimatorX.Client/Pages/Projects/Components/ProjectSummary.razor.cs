using EstimatorX.Client.Repositories;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Pages.Projects.Components;

public partial class ProjectSummary<TStore, TRepository, TModel>
    where TStore : StoreEditBase<TModel, TRepository>
    where TRepository : RepositoryEditBase<TModel>
    where TModel : Project, IHaveIdentifier, new()
{
    private Dictionary<string, bool> _collapseState = new Dictionary<string, bool>();

    private bool CollapseState(string id)
    {
        _collapseState.TryGetValue(id, out bool state);
        return state;
    }

    private bool ToggleCollapse(string id)
    {
        _collapseState.TryGetValue(id, out bool state);

        state = !state;
        _collapseState[id] = state;

        return state;
    }
}
