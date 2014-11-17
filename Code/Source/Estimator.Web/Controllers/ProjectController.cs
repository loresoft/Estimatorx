using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Estimator.Core;

namespace Estimator.Web.Controllers
{
    public class ProjectController : ApiController
    {
        // GET: api/Project
        public IEnumerable<Project> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Project/5
        public Project Get(Guid id)
        {
            return "value";
        }

        // POST: api/Project
        public void Post([FromBody]Project value)
        {
        }

        // PUT: api/Project/5
        public void Put(Guid id, [FromBody]Project value)
        {
        }

        // DELETE: api/Project/5
        public void Delete(Guid id)
        {
        }
    }
}
