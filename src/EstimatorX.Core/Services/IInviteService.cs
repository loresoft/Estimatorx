using System.Security.Principal;

using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Services;

public interface IInviteService
{
    Task<Invite> Create(Invite model, IPrincipal principal, CancellationToken cancellationToken);

    Task<bool> Redeem(string securityKey, IPrincipal principal, CancellationToken cancellationToken);

    Task<Invite> Save(string id, string partitionKey, Invite model, IPrincipal principal, CancellationToken cancellationToken);

    Task<bool> Send(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken);

    Task Delete(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken);

    Task<Invite> Load(string id, string partitionKey, IPrincipal principal, CancellationToken cancellationToken);

    Task<Invite> LoadBySecurityKey(string securityKey, IPrincipal principal, CancellationToken cancellationToken);
}
