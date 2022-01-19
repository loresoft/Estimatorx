using System.Net.Mime;

using EstimatorX.Core.Services;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EstimatorX.Service.Controllers;

[Authorize]
[ApiController]
[Route("api/administrative")]
[Produces(MediaTypeNames.Application.Json)]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class AdministrativeController : ControllerBase
{
    private readonly IAdministrativeService _administrativeService;

    public AdministrativeController(IAdministrativeService administrativeService)
    {
        _administrativeService = administrativeService;
    }

    [HttpGet("organizations/{id}/{partitionKey?}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public virtual async Task<ActionResult<Organization>> LoadOrganization(CancellationToken cancellationToken, [FromRoute] string id, [FromRoute] string partitionKey = null)
    {
        var result = await _administrativeService.LoadOrganization(id, partitionKey, User, cancellationToken);

        return Ok(result);
    }

    [HttpGet("organizations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public virtual async Task<ActionResult<QueryResult<OrganizationSummary>>> SearchOrganizations(CancellationToken cancellationToken, [FromQuery] QueryRequest queryRequest)
    {
        var results = await _administrativeService.SearchOrganizations(queryRequest, User, cancellationToken);

        return Ok(results);
    }


    [HttpGet("users/{id}/{partitionKey?}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public virtual async Task<ActionResult<User>> LoadUser(CancellationToken cancellationToken, [FromRoute] string id, [FromRoute] string partitionKey = null)
    {
        var result = await _administrativeService.LoadUser(id, partitionKey, User, cancellationToken);

        return Ok(result);
    }


    [HttpGet("users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public virtual async Task<ActionResult<QueryResult<UserSummary>>> SearchUsers(CancellationToken cancellationToken, [FromQuery] QueryRequest queryRequest)
    {
        var results = await _administrativeService.SearchUsers(queryRequest, User, cancellationToken);

        return Ok(results);
    }
}
