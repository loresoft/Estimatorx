
using Cosmos.Abstracts;

using EstimatorX.Core.Services;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EstimatorX.Core.Repositories;

[RegisterSingleton]
public class UserRepository
    : CosmosRepository<User>, IUserRepository
{
    private readonly IMemoryCache _memoryCache;

    public UserRepository(ILoggerFactory logFactory, IOptions<CosmosRepositoryOptions> repositoryOptions, ICosmosFactory databaseFactory, IMemoryCache memoryCache)
        : base(logFactory, repositoryOptions, databaseFactory)
    {
        _memoryCache = memoryCache;
    }

    public async Task<IReadOnlyList<User>> OrganizationMembersAsync(string organizationId, CancellationToken cancellationToken = default)
    {
        return await FindAllAsync(u => u.Organizations.Any(o => o.Id == organizationId), cancellationToken);
    }

    protected override void BeforeSave(User entity)
    {
        base.BeforeSave(entity);

        if (entity.Id.IsNullOrEmpty())
            entity.Id = ObjectId.GenerateNewId().ToString();

        if (entity.PrivateKey.IsNullOrEmpty())
            entity.PrivateKey = ObjectId.GenerateNewId().ToString();

        if (entity.Created == default)
            entity.Created = DateTimeOffset.UtcNow;

        entity.Updated = DateTimeOffset.UtcNow;
    }

    protected override void AfterSave(User entity)
    {
        base.AfterSave(entity);

        var userId = entity.Id;
        var key = UserCache.CreateKey(userId);

        _memoryCache.Remove(key);
    }
}
