using System.Security.Principal;

using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Services;

public interface IService<TModel>
{
    Task Delete(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken);
    Task<TModel> Load(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken);
    Task<TModel> Save(string id, string partitionKey, TModel model, IPrincipal principal, CancellationToken cancellationToken);
    Task<TModel> Create(TModel model, IPrincipal principal, CancellationToken cancellationToken);
    Task<QueryResult<TResult>> Search<TResult>(QueryRequest queryModel, IPrincipal principal, CancellationToken cancellationToken);
}
