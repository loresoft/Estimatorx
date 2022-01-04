using System.Net.Mime;

using EstimatorX.Core.Services;
using EstimatorX.Shared.Models;

using Microsoft.AspNetCore.Mvc;

namespace EstimatorX.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class ServiceControllerBase<TService, TModel, TList> : ControllerBase
    where TService : IService<TModel>
{

    public ServiceControllerBase(TService service)
    {
        Service = service;
    }

    protected TService Service { get; }


    [HttpDelete("{id}/{partitionKey?}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public virtual async Task<ActionResult> Delete(CancellationToken cancellationToken, [FromRoute] string id, [FromRoute] string partitionKey = null)
    {
        await Service.Delete(id, partitionKey, User, cancellationToken);

        return NoContent();
    }

    [HttpGet("{id}/{partitionKey?}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public virtual async Task<ActionResult<TModel>> Load(CancellationToken cancellationToken, [FromRoute] string id, [FromRoute] string partitionKey = null)
    {
        var result = await Service.Load(id, partitionKey, User, cancellationToken);

        return Ok(result);
    }

    [HttpPost("{id}/{partitionKey?}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public virtual async Task<ActionResult<TModel>> Save(CancellationToken cancellationToken, [FromBody] TModel model, [FromRoute] string id, [FromRoute] string partitionKey = null)
    {
        var result = await Service.Save(id, partitionKey, model, User, cancellationToken);

        return Ok(result);
    }

    [HttpPost("")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public virtual async Task<ActionResult<TModel>> Create(CancellationToken cancellationToken, [FromBody] TModel model)
    {
        var result = await Service.Create(model, User, cancellationToken);

        return Ok(result);
    }

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public virtual async Task<ActionResult<QueryResult<TList>>> Search(CancellationToken cancellationToken, [FromQuery] QueryRequest queryRequest)
    {
        var results = await Service.Search<TList>(queryRequest, User, cancellationToken);

        return Ok(results);
    }
}
