using Cosmos.Abstracts;

using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Repositories;

public interface IUserRepository
    : ICosmosRepository<User>
{
    Task<IReadOnlyList<User>> OrganizationMembersAsync(string organizationId, CancellationToken cancellationToken = default);
}
