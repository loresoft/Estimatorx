using EstimatorX.Core.Services;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Mvc;

namespace EstimatorX.Service.Controllers;

public class InviteController : ServiceControllerBase<InviteService, Invite, InviteSummary>
{
    public InviteController(InviteService service) : base(service)
    {
    }

    [HttpPost("{id}/{partitionKey}/send")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public virtual async Task<ActionResult<bool>> Send(CancellationToken cancellationToken, [FromRoute] string id, [FromRoute] string partitionKey)
    {
        var result = await Service.Send(id, partitionKey, User, cancellationToken);

        return Ok(result);
    }

    [HttpPost("key/{securityKey}/redeem")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public virtual async Task<ActionResult<bool>> Redeem(CancellationToken cancellationToken, [FromRoute] string securityKey)
    {
        var result = await Service.Redeem(securityKey, User, cancellationToken);

        return Ok(result);
    }

    [HttpGet("key/{securityKey}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public virtual async Task<ActionResult<Invite>> LoadBySecurityKey(CancellationToken cancellationToken, [FromRoute] string securityKey)
    {
        var result = await Service.LoadBySecurityKey(securityKey, User, cancellationToken);

        return Ok(result);
    }
}
