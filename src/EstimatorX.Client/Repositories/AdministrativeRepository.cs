using EstimatorX.Client.Pages.Projects;
using EstimatorX.Client.Services;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using FluentRest;

namespace EstimatorX.Client.Repositories;

public class AdministrativeRepository : IServiceScoped
{
    protected GatewayClient Gateway { get; }

    public AdministrativeRepository(GatewayClient gateway)
    {
        Gateway = gateway;
    }

    public async Task<User> LoadUser(string id, string partitionKey = null)
    {
        if (id is null)
            throw new ArgumentNullException(nameof(id));

        var result = await Gateway.GetAsync<User>(b => b
            .AppendPath("/api/administrative/users")
            .AppendPath(id)
            .AppendPathIf(partitionKey.HasValue, partitionKey)
        );

        return result;
    }

    public async Task<QueryResult<UserSummary>> SearchUsers(QueryRequest queryRequest)
    {
        if (queryRequest is null)
            throw new ArgumentNullException(nameof(queryRequest));

        var result = await Gateway.GetAsync<QueryResult<UserSummary>>(b => b
            .AppendPath("/api/administrative/users")
            .QueryStringIf(() => queryRequest.Page != 1, nameof(QueryRequest.Page), queryRequest.Page)
            .QueryStringIf(() => queryRequest.PageSize != 20, nameof(QueryRequest.PageSize), queryRequest.PageSize)
            .QueryStringIf(queryRequest.Sort.HasValue, nameof(QueryRequest.Sort), queryRequest.Sort)
            .QueryStringIf(() => queryRequest.Descending == true, nameof(QueryRequest.Descending), queryRequest.Descending)
            .QueryStringIf(queryRequest.Search.HasValue, nameof(QueryRequest.Search), queryRequest.Search)
            .QueryStringIf(queryRequest.Organization.HasValue, nameof(QueryRequest.Organization), queryRequest.Organization)
        );

        return result;
    }

    public async Task<Organization> LoadOrganization(string id, string partitionKey = null)
    {
        if (id is null)
            throw new ArgumentNullException(nameof(id));

        var result = await Gateway.GetAsync<Organization>(b => b
            .AppendPath("/api/administrative/organizations")
            .AppendPath(id)
            .AppendPathIf(partitionKey.HasValue, partitionKey)
        );

        return result;
    }

    public async Task<QueryResult<OrganizationSummary>> SearchOrganizations(QueryRequest queryRequest)
    {
        if (queryRequest is null)
            throw new ArgumentNullException(nameof(queryRequest));

        var result = await Gateway.GetAsync<QueryResult<OrganizationSummary>>(b => b
            .AppendPath("/api/administrative/organizations")
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
