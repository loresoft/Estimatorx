using EstimatorX.Shared.Models;
using MediatR;
using MediatR.CommandQuery.Mvc;

namespace EstimatorX.Service.Controllers
{
    public class OrganizationController
        : EntityCommandControllerBase<string, OrganizationModel, OrganizationModel, OrganizationModel, OrganizationModel>
    {
        public OrganizationController(IMediator mediator) : base(mediator)
        {
        }
    }
}
