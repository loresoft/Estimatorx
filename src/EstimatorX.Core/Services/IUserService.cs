using System.Security.Principal;

using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Services;

public interface IUserService
{
    Task Delete(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken);
    Task<User> Load(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken);
    Task<UserProfile> Login(BrowserDetail browserDetail, IPrincipal principal, CancellationToken cancellationToken);
    Task<User> Save(string id, string partitionKey, User model, IPrincipal principal, CancellationToken cancellationToken);
    Task<QueryResult<TResult>> Search<TResult>(QueryRequest queryModel, IPrincipal principal, CancellationToken cancellationToken);
}
