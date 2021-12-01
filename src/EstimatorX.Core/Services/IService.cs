using System.Security.Principal;

using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Services;

public interface IService<TModel>
{
    Task Delete(string id, IPrincipal principal, CancellationToken cancellationToken);
    Task<TModel> Load(string id, IPrincipal principal, CancellationToken cancellationToken);
    Task<TModel> Save(TModel model, IPrincipal principal, CancellationToken cancellationToken);
    Task<QueryResult<TResult>> Search<TResult>(QueryRequest queryModel, IPrincipal principal, CancellationToken cancellationToken);
}