using System.Security.Principal;

using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Services;

public interface IAdministrativeService
{
    Task<Organization> LoadOrganization(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken);
    Task<User> LoadUser(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken);
    Task<QueryResult<OrganizationSummary>> SearchOrganizations(QueryRequest queryRequest, IPrincipal principal, CancellationToken cancellationToken);
    Task<QueryResult<UserSummary>> SearchUsers(QueryRequest queryRequest, IPrincipal principal, CancellationToken cancellationToken);
}