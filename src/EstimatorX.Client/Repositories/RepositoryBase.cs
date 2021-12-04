
using EstimatorX.Client.Services;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using FluentRest;

namespace EstimatorX.Client.Repositories;

public abstract class RepositoryBase<TModel, TSummary>
{
    protected RepositoryBase(GatewayClient gateway)
    {
        Gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));
    }

    protected GatewayClient Gateway { get; }


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

    public async Task<QueryResult<TSummary>> Search(QueryRequest queryRequest)
    {
        if (queryRequest is null)
            throw new ArgumentNullException(nameof(queryRequest));

        var result = await Gateway.GetAsync<QueryResult<TSummary>>(b => b
            .AppendPath(GetBasePath())
            .QueryStringIf(() => queryRequest.Page != 1, nameof(QueryRequest.Page), queryRequest.Page)
            .QueryStringIf(() => queryRequest.PageSize != 20, nameof(QueryRequest.PageSize), queryRequest.PageSize)
            .QueryStringIf(queryRequest.Sort.HasValue, nameof(QueryRequest.Sort), queryRequest.Sort)
            .QueryStringIf(() => queryRequest.Descending == true, nameof(QueryRequest.Descending), queryRequest.Descending)
            .QueryStringIf(queryRequest.Search.HasValue, nameof(QueryRequest.Search), queryRequest.Search)
            .QueryStringIf(queryRequest.Organization.HasValue, nameof(QueryRequest.Organization), queryRequest.Organization)
        );

        return result;
    }

    public async Task<TModel> Save(TModel model)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        var result = await Gateway.PostAsync<TModel>(b => b
            .AppendPath(GetBasePath())
            .Content(model)
        );

        return result;
    }

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


    protected abstract string GetBasePath();
}
