using Cosmos.Abstracts;

using EstimatorX.Core.Entities;
using EstimatorX.Shared.Definitions;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EstimatorX.Core.Repositories;

public class UserRepository
    : CosmosRepository<User>, IUserRepository, ISingletonService
{
    public UserRepository(ILoggerFactory logFactory, IOptions<CosmosRepositoryOptions> repositoryOptions, ICosmosFactory databaseFactory)
        : base(logFactory, repositoryOptions, databaseFactory)
    {

    }

    public async Task<IReadOnlyList<User>> OrganizationMembersAsync(string organizationId, CancellationToken cancellationToken = default)
    {
        return await FindAllAsync(u => u.Organizations.Any(o => o.Id == organizationId), cancellationToken);
    }
}
