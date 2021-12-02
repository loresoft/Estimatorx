using Cosmos.Abstracts;

using EstimatorX.Core.Services;
using EstimatorX.Shared.Definitions;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EstimatorX.Core.Repositories;

public class UserRepository
    : CosmosRepository<Entities.User>, IUserRepository, ISingletonService
{
    private readonly IMemoryCache _memoryCache;

    public UserRepository(ILoggerFactory logFactory, IOptions<CosmosRepositoryOptions> repositoryOptions, ICosmosFactory databaseFactory, IMemoryCache memoryCache)
        : base(logFactory, repositoryOptions, databaseFactory)
    {
        _memoryCache = memoryCache;
    }

    public async Task<IReadOnlyList<Entities.User>> OrganizationMembersAsync(string organizationId, CancellationToken cancellationToken = default)
    {
        return await FindAllAsync(u => u.Organizations.Any(o => o.Id == organizationId), cancellationToken);
    }

    protected override void AfterSave(Entities.User entity)
    {
        base.AfterSave(entity);

        var userId = entity.Id;
        var key = UserCache.CreateKey(userId);

        _memoryCache.Remove(key);
    }
}
