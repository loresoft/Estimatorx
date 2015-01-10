using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Estimatorx.Core;
using Estimatorx.Core.Providers;
using Estimatorx.Core.Query;
using Estimatorx.Core.Security;
using Estimatorx.Data.Mongo;
using Estimatorx.Data.Mongo.Security;
using Microsoft.AspNet.Identity;
using MongoDB.Driver.Linq;

namespace Estimatorx.Web.Services
{
    [Authorize]
    [RoutePrefix("api/Project")]
    public class ProjectController : ApiController
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public ProjectController()
            : this(new ProjectRepository(), new UserRepository())
        {
        }

        public ProjectController(IProjectRepository projectRepository, IUserRepository userRepository)
        {
            if (projectRepository == null)
                throw new ArgumentNullException("projectRepository");
            if (userRepository == null)
                throw new ArgumentNullException("userRepository");

            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("Query")]
        public IHttpActionResult Query(int? page = null, int? pageSize = null, string sort = null, bool? descending = null)
        {
            var query = SecureQuery();
            if (query == null)
                return Unauthorized();

            var result = query
                .ToDataResult<Project, ProjectSummary>(config => config
                    .Page(page ?? 1)
                    .PageSize(pageSize ?? 50)
                    .Sort(sort)
                    .Descending(descending ?? false)
                    .Selector(ProjectRepository.SelectSummary())
                );

            return Ok(result);
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            var query = SecureQuery();
            if (query == null)
                return Unauthorized();

            return Ok(query);
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            Project project;
            if (!HasAccess(id, out project))
                return Unauthorized();

            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]Project value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!HasAccess(value.Id))
                return Unauthorized();

            var project = _projectRepository.Save(value);
            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            if (!HasAccess(id))
                return Unauthorized();

            _projectRepository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);           
        }

        private IQueryable<Project> SecureQuery()
        {
            var currentUser = _userRepository.Find(User.Identity.GetUserId());
            if (currentUser == null)
                return null;

            // access to organizations or user id
            var access = currentUser.Organizations;
            access.Add(currentUser.Id);

            return _projectRepository.All()
                .Where(t => t.OrganizationId.In(access));
        }

        private bool HasAccess(string id)
        {
            Project template;
            return HasAccess(id, out template);
        }

        private bool HasAccess(string id, out Project project)
        {
            project = null;
            string userId = User.Identity.GetUserId();
            var user = _userRepository.Find(userId);
            if (user == null)
                return false;

            project = _projectRepository.Find(id);
            if (project == null)
                return true; // allow create

            // user must be member 
            return project.OrganizationId == user.Id 
                || user.Organizations.Contains(project.OrganizationId);
        }

    }
}
