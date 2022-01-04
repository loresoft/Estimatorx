using EstimatorX.Core.Services;
using EstimatorX.Shared.Models;

namespace EstimatorX.Service.Controllers;

public class ProjectController : ServiceControllerBase<ProjectService, Project, ProjectSummary>
{
    public ProjectController(ProjectService service) : base(service)
    {
    }
}
