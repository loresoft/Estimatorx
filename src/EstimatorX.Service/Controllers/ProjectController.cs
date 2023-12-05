using EstimatorX.Core.Services;
using EstimatorX.Shared.Models;

namespace EstimatorX.Service.Controllers;

public class ProjectController : ServiceControllerBase<IProjectService, Project, ProjectSummary>
{
    public ProjectController(IProjectService service) : base(service)
    {
    }
}
