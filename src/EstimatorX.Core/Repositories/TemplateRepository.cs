using Cosmos.Abstracts;
using EstimatorX.Core.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EstimatorX.Core.Repositories
{
    public class TemplateRepository
        : CosmosRepository<Template>, ITemplateRepository
    {
        public TemplateRepository(ILoggerFactory logFactory, IOptions<CosmosRepositoryOptions> repositoryOptions, ICosmosFactory databaseFactory)
            : base(logFactory, repositoryOptions, databaseFactory)
        {

        }
    }
}
