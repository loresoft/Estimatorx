
using EstimatorX.Client.Services;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Repositories;

public class ProjectRepository : RepositorySearchBase<Project, ProjectSummary>, IScopedService
{
    public ProjectRepository(GatewayClient gateway) : base(gateway)
    {
    }

    protected override string GetBasePath()
    {
        return "/api/project";
    }
}
