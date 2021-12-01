using EstimatorX.Core.Services;
using EstimatorX.Shared.Models;

namespace EstimatorX.Service.Controllers;

public class TemplateController : ServiceControllerBase<TemplateService, TemplateModel, TemplateSummary>
{
    public TemplateController(TemplateService service) : base(service)
    {
    }
}
