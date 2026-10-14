using Cosmos.Abstracts;
using Cosmos.Abstracts.Extensions;

using Estimatorx.Data.Cosmos.Models;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Estimatorx.Data.Cosmos.Repositories;

[RegisterSingleton]
public class ProjectRepository
    : CosmosRepository<Project>, IProjectRepository
{
    public ProjectRepository(ILoggerFactory logFactory, IOptions<CosmosRepositoryOptions> repositoryOptions, ICosmosFactory databaseFactory)
        : base(logFactory, repositoryOptions, databaseFactory)
    {
    }

    protected override void BeforeSave(Project entity)
    {
        base.BeforeSave(entity);

        if (entity.Id.IsNullOrEmpty())
            entity.Id = ObjectId.GenerateNewId().ToString();

        if (entity.SecurityKey.IsNullOrWhiteSpace())
            entity.SecurityKey = Guid.NewGuid().ToString("N");

        if (entity.Created == default)
            entity.Created = DateTimeOffset.UtcNow;

        //entity.Updated = DateTimeOffset.UtcNow;
    }

    protected override string GetEntityKey(Project entity)
    {
        return entity.Id;
    }

    protected override PartitionKey GetPartitionKey(Project entity)
    {
        return new PartitionKey(entity.OrganizationId);
    }

    protected override string GetPartitionKeyPath()
    {
        return "/organizationId";
    }
}
