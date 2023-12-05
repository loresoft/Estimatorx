using EstimatorX.Core.Services;
using EstimatorX.Shared.Models;

namespace EstimatorX.Service.Controllers;

public class TemplateController : ServiceControllerBase<ITemplateService, Template, TemplateSummary>
{
    public TemplateController(ITemplateService service) : base(service)
    {
    }
}
