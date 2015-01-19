using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Estimatorx.Data.Mongo;
using Estimatorx.Core;
using Estimatorx.Core.Providers;
using Estimatorx.Core.Query;
using Estimatorx.Core.Security;
using Estimatorx.Data.Mongo.Security;
using Microsoft.AspNet.Identity;
using MongoDB.Driver.Linq;

namespace Estimatorx.Web.Services
{
    [Authorize]
    [RoutePrefix("api/Template")]
    public class TemplateController : ApiController
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IUserRepository _userRepository;

        public TemplateController()
            : this(new TemplateRepository(), new UserRepository())
        {
        }

        public TemplateController(ITemplateRepository templateRepository, IUserRepository userRepository)
        {
            if (templateRepository == null)
                throw new ArgumentNullException("templateRepository");
            if (userRepository == null)
                throw new ArgumentNullException("userRepository");

            _templateRepository = templateRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("Query")]
        public IHttpActionResult Query(int? page = null, int? pageSize = null, string sort = null, bool? descending = null, string search = null, string organization = null)
        {
            var query = SecureQuery();
            if (query == null)
                return Unauthorized();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Name.ToLower().Contains(search));

            if (!string.IsNullOrEmpty(organization))
                query = query.Where((p => p.OrganizationId == organization));

            var result = query
                .ToDataResult<Template, TemplateSummary>(config => config
                    .Page(page ?? 1)
                    .PageSize(pageSize ?? 50)
                    .Sort(sort)
                    .Descending(descending ?? false)
                    .Selector(TemplateRepository.SelectSummary())
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

            Template template;
            if (!HasAccess(id, out template))
                return Unauthorized();

            if (template == null)
                return NotFound();

            return Ok(template);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]Template value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!HasAccess(value.Id))
                return Unauthorized();

            var template = _templateRepository.Save(value);
            if (template == null)
                return NotFound();

            return Ok(template);
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            if (!HasAccess(id))
                return Unauthorized();
            
            _templateRepository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }


        private IQueryable<Template> SecureQuery()
        {
            var currentUser = _userRepository.Find(User.Identity.GetUserId());
            if (currentUser == null)
                return null;

            // access to organizations or user id
            var access = currentUser.Organizations;
            access.Add(currentUser.Id);

            return _templateRepository
                .All()
                .Where(t => t.OrganizationId.In(access));
        }

        private bool HasAccess(string id)
        {
            Template template;
            return HasAccess(id, out template);
        }

        private bool HasAccess(string id, out Template template)
        {
            template = null;
            string userId = User.Identity.GetUserId();
            var user = _userRepository.Find(userId);
            if (user == null)
                return false;

            template = _templateRepository.Find(id);
            if (template == null)
                return true; // allow create

            // user must be member 
            return template.OrganizationId == user.Id
                || user.Organizations.Contains(template.OrganizationId);
        }

    }
}
