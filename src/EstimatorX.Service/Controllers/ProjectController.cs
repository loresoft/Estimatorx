using EstimatorX.Shared.Models;
using MediatR;
using MediatR.CommandQuery.Mvc;

namespace EstimatorX.Service.Controllers
{
    public class ProjectController
        : EntityCommandControllerBase<string, ProjectModel, ProjectModel, ProjectModel, ProjectModel>
    {
        public ProjectController(IMediator mediator) : base(mediator)
        {
        }
    }
}
