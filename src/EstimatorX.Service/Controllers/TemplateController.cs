using EstimatorX.Shared.Models;
using MediatR;
using MediatR.CommandQuery.Mvc;

namespace EstimatorX.Service.Controllers;

public class TemplateController
    : EntityCommandControllerBase<string, TemplateModel, TemplateModel, TemplateModel, TemplateModel>
{
    public TemplateController(IMediator mediator) : base(mediator)
    {
    }
}
