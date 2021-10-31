using Cosmos.Abstracts;
using EstimatorX.Core.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EstimatorX.Core.Repositories;

public class ProjectRepository
    : CosmosRepository<Project>, IProjectRepository
{
    public ProjectRepository(ILoggerFactory logFactory, IOptions<CosmosRepositoryOptions> repositoryOptions, ICosmosFactory databaseFactory)
        : base(logFactory, repositoryOptions, databaseFactory)
    {

    }
}
