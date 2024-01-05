using System.Net.Mime;

using EstimatorX.Core.Options;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EstimatorX.Service.Controllers;

[Authorize]
[ApiController]
[Route("api/administrative/logging")]
[Produces(MediaTypeNames.Application.Json)]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class LoggingController : ControllerBase
{
    private readonly IOptions<LoggingOptions> _options;

    public LoggingController(IOptions<LoggingOptions> options)
    {
        _options = options;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public ActionResult<IReadOnlyCollection<string>> List()
    {
        var results = Directory
            .EnumerateFiles(_options.Value.Path, _options.Value.Filter)
            .Select(Path.GetFileNameWithoutExtension)
            .OrderByDescending(s => s)
            .ToList();

        return Ok(results);
    }

    [HttpGet("{file}")]
    [Produces("application/vnd.serilog.clef")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public ActionResult Read(string file)
    {
        var path = Path.Combine(_options.Value.Path, file) + ".clef";
        path = Path.GetFullPath(path);

        if (!System.IO.File.Exists(path))
            return NotFound("File Not found");

        var readStream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        return File(readStream, "application/vnd.serilog.clef");
    }
}
