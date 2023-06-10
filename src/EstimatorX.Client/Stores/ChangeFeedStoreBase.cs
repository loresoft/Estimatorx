using System.Text.Json;

using EstimatorX.Client.Repositories;
using EstimatorX.Shared.Changes;
using EstimatorX.Shared.Definitions;

using Json.Patch;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace EstimatorX.Client.Stores;

public abstract class ChangeFeedStoreBase<TModel, TRepository>
    : StoreEditBase<TModel, TRepository>, IAsyncDisposable
    where TRepository : RepositoryEditBase<TModel>
    where TModel : class, IHaveIdentifier, new()
{
    protected ChangeFeedStoreBase(
        ILoggerFactory loggerFactory,
        TRepository repository,
        JsonSerializerOptions serializerOptions,
        NavigationManager navigation)
        : base(loggerFactory, repository, serializerOptions)
    {
        Navigation = navigation;
    }

    public HubConnection HubConnection { get; private set; }

    public NavigationManager Navigation { get; }

    public abstract string ChangeEventName { get; }

    public async Task InitializedAsync()
    {
        if (HubConnection != null)
            return;

        Logger.LogInformation("Initialize project change feed {url}", ChangeFeedConstants.HubPath);

        var url = Navigation.ToAbsoluteUri(ChangeFeedConstants.HubPath);

        HubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();

        HubConnection.On<TModel>(ChangeEventName, ModelChanged);

        await HubConnection.StartAsync();
    }

    protected virtual void ModelChanged(TModel model)
    {
        // ignore changes not for this model
        if (model == null || model.Id != Original?.Id)
            return;

        Logger.LogInformation("Model {Id} changed", model?.Id);

        // compare from original because current might have inprogress changes
        var patch = Original.CreatePatch(model, SerializerOptions);

        if (patch.Operations.Count == 0)
            return;

        Logger.LogInformation("Applying {Count} patches to model {Id}", patch.Operations.Count, model?.Id);

        // apply to current model
        Model = patch.Apply(Model, SerializerOptions);

        // replace original
        SetOriginal(model);

        NotifyStateChanged();
    }

    public async ValueTask DisposeAsync()
    {
        if (HubConnection is not null)
            await HubConnection.DisposeAsync();

        HubConnection = null;
    }
}
