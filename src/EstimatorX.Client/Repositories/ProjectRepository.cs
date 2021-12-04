
using EstimatorX.Client.Services;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Repositories;

public class ProjectRepository : RepositoryBase<ProjectModel, ProjectSummary>
{
    public ProjectRepository(GatewayClient gateway) : base(gateway)
    {
    }

    protected override string GetBasePath()
    {
        return "/api/project";
    }
}
