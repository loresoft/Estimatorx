using Cosmos.Abstracts;

using EstimatorX.Core.Entities;
using EstimatorX.Shared.Definitions;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EstimatorX.Core.Repositories;

public class OrganizationRepository
    : CosmosRepository<Organization>, IOrganizationRepository, ISingletonService
{
    public OrganizationRepository(ILoggerFactory logFactory, IOptions<CosmosRepositoryOptions> repositoryOptions, ICosmosFactory databaseFactory)
        : base(logFactory, repositoryOptions, databaseFactory)
    {

    }
}
