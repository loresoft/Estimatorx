using System.Security.Principal;

using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Services;

public interface IUserService
{
    Task Delete(string id, IPrincipal principal, CancellationToken cancellationToken);
    Task<UserModel> Load(string id, IPrincipal principal, CancellationToken cancellationToken);
    Task<UserModel> Login(BrowserDetail browserDetail, IPrincipal principal, CancellationToken cancellationToken);
    Task<UserModel> Save(UserModel model, IPrincipal principal, CancellationToken cancellationToken);
    Task<QueryResult<TResult>> Search<TResult>(QueryRequest queryModel, IPrincipal principal, CancellationToken cancellationToken);
}