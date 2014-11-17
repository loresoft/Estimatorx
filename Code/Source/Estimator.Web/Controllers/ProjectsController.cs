using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.OData.Routing;
using Estimator.Core;
using Microsoft.Data.OData;

namespace Estimator.Web.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Estimator.Core;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Project>("Projects");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ProjectsController : ODataController
    {
        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        // GET: odata/Projects
        public async Task<IHttpActionResult> GetProjects(ODataQueryOptions<Project> queryOptions)
        {
            // validate the query.
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            // return Ok<IEnumerable<Project>>(projects);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // GET: odata/Projects(5)
        public async Task<IHttpActionResult> GetProject([FromODataUri] System.Guid key, ODataQueryOptions<Project> queryOptions)
        {
            // validate the query.
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            // return Ok<Project>(project);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PUT: odata/Projects(5)
        public async Task<IHttpActionResult> Put([FromODataUri] System.Guid key, Delta<Project> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Put(project);

            // TODO: Save the patched entity.

            // return Updated(project);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // POST: odata/Projects
        public async Task<IHttpActionResult> Post(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.

            // return Created(project);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // PATCH: odata/Projects(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] System.Guid key, Delta<Project> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.

            // delta.Patch(project);

            // TODO: Save the patched entity.

            // return Updated(project);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // DELETE: odata/Projects(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] System.Guid key)
        {
            // TODO: Add delete logic here.

            // return StatusCode(HttpStatusCode.NoContent);
            return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}
