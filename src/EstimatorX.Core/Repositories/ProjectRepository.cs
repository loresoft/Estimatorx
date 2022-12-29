using Cosmos.Abstracts;

using EstimatorX.Core.Services;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EstimatorX.Core.Repositories;

[RegisterSingleton]
public class ProjectRepository
    : CosmosRepository<Project>, IProjectRepository
{
    private readonly ISecurityKeyGenerator _securityKeyGenerator;

    public ProjectRepository(ILoggerFactory logFactory, IOptions<CosmosRepositoryOptions> repositoryOptions, ICosmosFactory databaseFactory, ISecurityKeyGenerator securityKeyGenerator)
        : base(logFactory, repositoryOptions, databaseFactory)
    {
        _securityKeyGenerator = securityKeyGenerator;
    }

    protected override void BeforeSave(Project entity)
    {
        base.BeforeSave(entity);

        if (entity.Id.IsNullOrEmpty())
            entity.Id = ObjectId.GenerateNewId().ToString();

        if (entity.SecurityKey.IsNullOrWhiteSpace())
            entity.SecurityKey = _securityKeyGenerator.GenerateKey();

        if (entity.Created == default)
            entity.Created = DateTimeOffset.UtcNow;

        entity.Updated = DateTimeOffset.UtcNow;
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
