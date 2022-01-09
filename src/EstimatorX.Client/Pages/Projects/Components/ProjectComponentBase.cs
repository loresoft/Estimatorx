using EstimatorX.Client.Repositories;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Definitions;

using Microsoft.AspNetCore.Components;

namespace EstimatorX.Client.Pages.Projects.Components;

public abstract class ProjectComponentBase<TStore, TRepository, TModel>
    : ComponentBase, IDisposable
    where TStore : StoreEditBase<TModel, TRepository>
    where TRepository : RepositoryEditBase<TModel>
    where TModel : class, IHaveIdentifier, new()
{
    [Inject]
    public TStore Store { get; set; }

    public TModel Model => Store.Model;

    protected override void OnInitialized()
    {
        Store.OnChange += HandleModelChange;
    }

    private void HandleModelChange()
    {
        InvokeAsync(() => StateHasChanged());
    }

    public virtual void Dispose()
    {
        Store.OnChange -= HandleModelChange;
    }

}
