using EstimatorX.Client.Repositories;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Stores;

[RegisterScoped]
public class ProjectStore : StoreEditBase<Project, ProjectRepository>
{

    public ProjectStore(ILoggerFactory loggerFactory, ProjectRepository projectRepository)
        : base(loggerFactory, projectRepository)
    {
    }

}
