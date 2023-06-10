using System.Security.Principal;

using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Services;

public interface IInviteService : IService<Invite>
{
    Task<bool> Redeem(string securityKey, IPrincipal principal, CancellationToken cancellationToken);

    Task<bool> Send(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken);

    Task<Invite> LoadBySecurityKey(string securityKey, IPrincipal principal, CancellationToken cancellationToken);
}
