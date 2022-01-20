using EstimatorX.Client.Repositories;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Stores;

public class ProjectStore : StoreEditBase<Project, ProjectRepository>, IServiceScoped
{

    public ProjectStore(ILoggerFactory loggerFactory, ProjectRepository projectRepository)
        : base(loggerFactory, projectRepository)
    {
    }

}
