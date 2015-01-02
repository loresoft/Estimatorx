using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Estimatorx.Data.Mongo;
using Estimatorx.Core;
using Estimatorx.Core.Providers;

namespace Estimatorx.Web.Services
{
    [Authorize]
    public class TemplateController : ApiController
    {
        private readonly ITemplateRepository _repository;

        public TemplateController()
            : this(new TemplateRepository())
        {
        }

        public TemplateController(ITemplateRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _repository = repository;
        }

        public IEnumerable<Template> Get()
        {
            return _repository.All();
        }

        public IHttpActionResult Get(string id)
        {
            var project = _repository.Find(id);
            if (project == null)
                return NotFound();

            return Ok(project);
        }

        public IHttpActionResult Post([FromBody]Template value)
        {
            var project = _repository.Save(value);
            if (project == null)
                return NotFound();

            return Ok(project);
        }

        public IHttpActionResult Delete(string id)
        {
            _repository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
