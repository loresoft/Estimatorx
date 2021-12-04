using System.Security.Principal;

using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Services;

public interface IOrganizationService
{
    Task Delete(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken);

    Task<OrganizationModel> Load(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken);

    Task<OrganizationModel> Save(OrganizationModel model, IPrincipal principal, CancellationToken cancellationToken);

    Task<QueryResult<TResult>> Search<TResult>(QueryRequest queryModel, IPrincipal principal, CancellationToken cancellationToken);
}
