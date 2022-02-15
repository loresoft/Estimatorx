
using Cosmos.Abstracts;

using EstimatorX.Core.Services;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Extensions;
using EstimatorX.Shared.Models;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EstimatorX.Core.Repositories;

public class InviteRepository
    : CosmosRepository<Invite>, IInviteRepository, IServiceSingleton
{
    private readonly ISecurityKeyGenerator _securityKeyGenerator;

    public InviteRepository(ILoggerFactory logFactory, IOptions<CosmosRepositoryOptions> repositoryOptions, ICosmosFactory databaseFactory, ISecurityKeyGenerator securityKeyGenerator)
        : base(logFactory, repositoryOptions, databaseFactory)
    {
        _securityKeyGenerator = securityKeyGenerator;
    }

    protected override void BeforeSave(Invite entity)
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

    protected override PartitionKey GetPartitionKey(Invite entity)
    {
        return new PartitionKey(entity.OrganizationId);
    }

    protected override string GetPartitionKeyPath()
    {
        return "/organizationId";
    }

    protected override ContainerProperties ContainerProperties()
    {
        var properties = base.ContainerProperties();

        var uniqueKey = new UniqueKey();
        uniqueKey.Paths.Add("/securityKey");

        var uniqueKeyPolicy = new UniqueKeyPolicy();
        uniqueKeyPolicy.UniqueKeys.Add(uniqueKey);

        properties.UniqueKeyPolicy = uniqueKeyPolicy;

        return properties;
    }
}
