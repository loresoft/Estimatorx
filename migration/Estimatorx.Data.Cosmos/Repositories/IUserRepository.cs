using Cosmos.Abstracts;

using Estimatorx.Data.Cosmos.Models;

namespace Estimatorx.Data.Cosmos.Repositories;

public interface IUserRepository
    : ICosmosRepository<User>
{
    Task<IReadOnlyList<User>> OrganizationMembersAsync(string organizationId, CancellationToken cancellationToken = default);
}
