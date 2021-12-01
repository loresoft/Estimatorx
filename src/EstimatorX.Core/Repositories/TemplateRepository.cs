using Cosmos.Abstracts;

using EstimatorX.Core.Entities;
using EstimatorX.Shared.Definitions;

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
}
