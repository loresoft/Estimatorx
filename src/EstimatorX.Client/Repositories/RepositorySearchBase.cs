
using EstimatorX.Client.Services;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using FluentRest;

namespace EstimatorX.Client.Repositories;

public abstract class RepositorySearchBase<TModel, TSummary> : RepositoryEditBase<TModel>
{
    protected RepositorySearchBase(GatewayClient gateway) : base(gateway)
    {
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
}
