
using EstimatorX.Core.Services;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Mvc;

namespace EstimatorX.Service.Controllers;

public class OrganizationController : ServiceControllerBase<OrganizationService, Organization, OrganizationSummary>
{
    public OrganizationController(OrganizationService service) : base(service)
    {
        
    }
}
