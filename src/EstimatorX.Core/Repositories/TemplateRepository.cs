using Cosmos.Abstracts;

using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EstimatorX.Core.Repositories;

public class TemplateRepository
    : CosmosRepository<Template>, ITemplateRepository, ISingletonService
{
    public TemplateRepository(ILoggerFactory logFactory, IOptions<CosmosRepositoryOptions> repositoryOptions, ICosmosFactory databaseFactory)
        : base(logFactory, repositoryOptions, databaseFactory)
    {

    }

    protected override void BeforeSave(Template entity)
    {
        base.BeforeSave(entity);

        if (entity.Id.IsNullOrEmpty())
            entity.Id = ObjectId.GenerateNewId().ToString();

        if (entity.Created == default)
            entity.Created = DateTimeOffset.UtcNow;

        entity.Updated = DateTimeOffset.UtcNow;
    }

    protected override PartitionKey GetPartitionKey(Template entity)
    {
        return new PartitionKey(entity.OrganizationId);
    }

    protected override string GetPartitionKeyPath()
    {
        return "/organizationId";
    }

}
