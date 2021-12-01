
using EstimatorX.Core.Services;
using EstimatorX.Shared.Models;

namespace EstimatorX.Service.Controllers;

public class OrganizationController : ServiceControllerBase<OrganizationService, OrganizationModel, OrganizationSummary>
{
    public OrganizationController(OrganizationService service) : base(service)
    {
    }
}
