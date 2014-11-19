using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Estimator.Core;
using Estimator.Core.Providers;
using Estimator.Data.Mongo;

namespace Estimator.Web.Controllers
{
    public class ProjectController : ApiController
    {
        private readonly IProjectRepository _repository;

        public ProjectController()
            : this(new ProjectRepository())
        {
        }

        public ProjectController(IProjectRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _repository = repository;
        }

        // GET: api/Project
        public IEnumerable<Project> Get()
        {
            return _repository.All();
        }

        // GET: api/Project/5
        public Project Get(Guid id)
        {
            return _repository.Find(id);
        }

        // POST: api/Project
        public HttpResponseMessage Post([FromBody]Project value)
        {
            _repository.Update(value);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // DELETE: api/Project/5
        public HttpResponseMessage Delete(Guid id)
        {
            _repository.Delete(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
