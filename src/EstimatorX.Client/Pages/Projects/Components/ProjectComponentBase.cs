using Blazored.Modal.Services;

using EstimatorX.Client.Extensions;
using EstimatorX.Client.Repositories;
using EstimatorX.Client.Stores;
using EstimatorX.Shared.Definitions;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace EstimatorX.Client.Pages.Projects.Components;

public abstract class ProjectComponentBase<TStore, TRepository, TModel>
    : ComponentBase, IDisposable
    where TStore : StoreEditBase<TModel, TRepository>
    where TRepository : RepositoryEditBase<TModel>
    where TModel : class, IHaveIdentifier, new()
{
    [CascadingParameter]
    public IModalService Modal { get; set; }

    [Inject]
    public TStore Store { get; set; }

    public TModel Model => Store.Model;

    protected override void OnInitialized()
    {
        Store.OnChange += HandleModelChange;
    }

    private void HandleModelChange()
    {
        InvokeAsync(StateHasChanged);
    }

    public virtual void Dispose()
    {
        Store.OnChange -= HandleModelChange;
    }

    protected virtual async Task ConfirmNavigation(LocationChangingContext context)
    {
        //if request is part of the current store model, no changes will not be lost.
        if (context.TargetLocation.Contains(Store.Model.Id))
            return;

        if (Store.IsClean)
            return;

        if (await Modal.ConfirmNavigation())
            return;

        context.PreventNavigation();
    }
}
