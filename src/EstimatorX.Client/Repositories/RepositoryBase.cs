
using EstimatorX.Client.Services;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using FluentRest;

using MediatR.CommandQuery.Queries;

namespace EstimatorX.Client.Repositories;

public abstract class RepositoryBase<TModel>
{
    protected RepositoryBase(GatewayClient gateway)
    {
        Gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));
    }

    protected GatewayClient Gateway { get; }


    public async Task<TModel> Get(string id)
    {
        if (id is null)
            throw new ArgumentNullException(nameof(id));

        var result = await Gateway.GetAsync<TModel>(b => b
            .AppendPath(GetBasePath())
            .AppendPath(id)
        );

        return result;
    }

    public async Task<IReadOnlyCollection<TModel>> All()
    {
        var result = await Gateway.GetAsync<List<TModel>>(b => b
            .AppendPath(GetBasePath())
        );

        return result;
    }

    public async Task<EntityPagedResult<TModel>> Page(EntityQuery entityQuery)
    {
        if (entityQuery is null)
            throw new ArgumentNullException(nameof(entityQuery));

        var result = await Gateway.PostAsync<EntityPagedResult<TModel>>(b => b
            .AppendPath(GetBasePath())
            .AppendPath("page")
            .Content(entityQuery)
        );

        return result;
    }

    public async Task<IReadOnlyCollection<TModel>> Select(EntitySelect entitySelect)
    {
        if (entitySelect is null)
            throw new ArgumentNullException(nameof(entitySelect));

        var result = await Gateway.PostAsync<IReadOnlyCollection<TModel>>(b => b
            .AppendPath(GetBasePath())
            .AppendPath("query")
            .Content(entitySelect)
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

    public async Task<TModel> Update(string id, TModel model)
    {
        if (id is null)
            throw new ArgumentNullException(nameof(id));

        if (model == null)
            throw new ArgumentNullException(nameof(model));

        var result = await Gateway.PutAsync<TModel>(b => b
            .AppendPath(GetBasePath())
            .AppendPath(id)
            .Content(model)
        );

        return result;
    }

    public async Task<TModel> Delete(string id)
    {
        if (id is null)
            throw new ArgumentNullException(nameof(id));

        var result = await Gateway.DeleteAsync<TModel>(b => b
            .AppendPath(GetBasePath())
            .AppendPath(id)
        );

        return result;
    }


    protected abstract string GetBasePath();
}
