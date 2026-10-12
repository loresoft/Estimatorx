
using Cosmos.Abstracts;
using Cosmos.Abstracts.Extensions;

using Estimatorx.Data.Cosmos.Models;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Estimatorx.Data.Cosmos.Repositories;

[RegisterSingleton]
public class UserRepository
    : CosmosRepository<User>, IUserRepository
{
    public UserRepository(ILoggerFactory logFactory, IOptions<CosmosRepositoryOptions> repositoryOptions, ICosmosFactory databaseFactory)
        : base(logFactory, repositoryOptions, databaseFactory)
    {
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

    protected override string GetEntityKey(User entity)
    {
        return entity.Id;
    }

    protected override void AfterSave(User entity)
    {
        base.AfterSave(entity);

        var userId = entity.Id;
    }
}
