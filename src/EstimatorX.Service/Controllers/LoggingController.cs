using System.Net.Mime;

using EstimatorX.Core.Services;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EstimatorX.Service.Controllers;

[Authorize]
[ApiController]
[Route("api/administrative/logging")]
[Produces(MediaTypeNames.Application.Json)]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class LoggingController : ControllerBase
{
    private readonly ILoggingService _loggingService;

    public LoggingController(ILoggingService loggingService)
    {
        _loggingService = loggingService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public virtual async Task<ActionResult<LogEventResult>> SearchUsers(CancellationToken cancellationToken, [FromQuery] LogEventRequest queryRequest)
    {
        var results = await _loggingService.Search(queryRequest, cancellationToken);

        return Ok(results);
    }
}
