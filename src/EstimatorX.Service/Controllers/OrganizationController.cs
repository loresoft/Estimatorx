
using EstimatorX.Core.Services;
using EstimatorX.Shared.Models;

namespace EstimatorX.Service.Controllers;

public class OrganizationController : ServiceControllerBase<IOrganizationService, Organization, OrganizationSummary>
{
    public OrganizationController(IOrganizationService service) : base(service)
    {

    }
}
