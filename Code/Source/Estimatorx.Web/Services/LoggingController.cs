using System;
using System.Web.Http;
using Estimatorx.Core.Providers;
using Estimatorx.Core.Query;
using Estimatorx.Core.Security;
using Estimatorx.Data.Mongo;

namespace Estimatorx.Web.Services
{
    [Authorize(Roles = RoleNames.Administrators)]
    [RoutePrefix("api/Logging")]
    public class LoggingController : ApiController
    {
        private readonly ILoggingRepository _loggingRepository;

        public LoggingController() : this(new LoggingRepository())
        {
        }

        public LoggingController(ILoggingRepository loggingRepository)
        {
            _loggingRepository = loggingRepository;
        }


        [HttpGet]
        [Route("Query")]
        public IHttpActionResult Query(int? page = null, int? pageSize = null, string sort = null, bool? descending = null, string filter = null)
        {
            var query = _loggingRepository.All();
            if (query == null)
                return Unauthorized();

            var result = query
                .ToDataResult(config => config
                    .Page(page ?? 1)
                    .PageSize(pageSize ?? 50)
                    .Sort(sort)
                    .Descending(descending ?? false)
                    .Filter(filter)
                );

            return Ok(result);
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var query = _loggingRepository.All();
            return Ok(query);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var log = _loggingRepository.Find(id);
            if (log == null)
                return NotFound();

            return Ok(log);
        }

    }
}
