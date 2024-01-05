using System.Security.Principal;

using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Services;

public interface IUserService : IService<User>
{
    Task<UserProfile> Login(BrowserDetail browserDetail, IPrincipal principal, CancellationToken cancellationToken);
}
