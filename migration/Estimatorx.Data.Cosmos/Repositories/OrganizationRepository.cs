using Cosmos.Abstracts;
using Cosmos.Abstracts.Extensions;

using Estimatorx.Data.Cosmos.Models;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Estimatorx.Data.Cosmos.Repositories;

[RegisterSingleton]
public class OrganizationRepository
    : CosmosRepository<Organization>, IOrganizationRepository
{
    public OrganizationRepository(ILoggerFactory logFactory, IOptions<CosmosRepositoryOptions> repositoryOptions, ICosmosFactory databaseFactory)
        : base(logFactory, repositoryOptions, databaseFactory)
    {

    }

    protected override void BeforeSave(Organization entity)
    {
        base.BeforeSave(entity);

        if (entity.Id.IsNullOrEmpty())
            entity.Id = ObjectId.GenerateNewId().ToString();

        if (entity.Created == default)
            entity.Created = DateTimeOffset.UtcNow;

        //entity.Updated = DateTimeOffset.UtcNow;
    }

    protected override string GetEntityKey(Organization entity)
    {
        return entity.Id;
    }
}
