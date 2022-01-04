using EstimatorX.Client.Services;
using EstimatorX.Shared.Extensions;

using FluentRest;

namespace EstimatorX.Client.Repositories;

public abstract class RepositoryEditBase<TModel>
{
    protected RepositoryEditBase(GatewayClient gateway)
    {
        Gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));
    }

    protected GatewayClient Gateway { get; }

    public async Task Delete(string id, string partitionKey = null)
    {
        if (id is null)
            throw new ArgumentNullException(nameof(id));

        var result = await Gateway.DeleteAsync(b => b
            .AppendPath(GetBasePath())
            .AppendPath(id)
            .AppendPathIf(partitionKey.HasValue, partitionKey)
        );

        result.EnsureSuccessStatusCode();

        return;
    }


    public async Task<TModel> Load(string id, string partitionKey = null)
    {
        if (id is null)
            throw new ArgumentNullException(nameof(id));

        var result = await Gateway.GetAsync<TModel>(b => b
            .AppendPath(GetBasePath())
            .AppendPath(id)
            .AppendPathIf(partitionKey.HasValue, partitionKey)
        );

        return result;
    }

    public async Task<TModel> Save(TModel model, string id, string partitionKey = null)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        var result = await Gateway.PostAsync<TModel>(b => b
            .AppendPath(GetBasePath())
            .AppendPath(id)
            .AppendPathIf(partitionKey.HasValue, partitionKey)
            .Content(model)
        );

        return result;
    }

    public async Task<TModel> Create(TModel model)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        var result = await Gateway.PostAsync<TModel>(b => b
            .AppendPath(GetBasePath())
            .Content(model)
        );

        return result;
    }
    protected abstract string GetBasePath();
}
