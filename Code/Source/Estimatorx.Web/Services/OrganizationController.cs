using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using Estimatorx.Core.Query;
using Estimatorx.Core.Security;
using Microsoft.AspNet.Identity;

namespace Estimatorx.Web.Services
{
    [Authorize]
    [RoutePrefix("api/Organization")]
    public class OrganizationController : ApiController
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;

        public OrganizationController(
            IOrganizationRepository organizationRepository,
            IUserRepository userRepository
        )
        {
            if (organizationRepository == null)
                throw new ArgumentNullException("organizationRepository");

            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("Query")]
        public IHttpActionResult Query(int? page = null, int? pageSize = null, string sort = null, bool? descending = null)
        {
            var currentUser = _userRepository.Find(User.Identity.GetUserId());
            if (currentUser == null)
                return Unauthorized();

            // find organization for current user only
            var result = _organizationRepository
                .FindAll(currentUser.Organizations)
                .ToDataResult(config => config
                    .Page(page ?? 1)
                    .PageSize(pageSize ?? 50)
                    .Sort(sort)
                    .Descending(descending ?? false)
                );

            return Ok(result);
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var currentUser = _userRepository.Find(User.Identity.GetUserId());
            if (currentUser == null)
                return NotFound();

            var organizations = _organizationRepository
                .FindAll(currentUser.Organizations)
                .ToList();

            // insert Private for self
            var item = new Organization
            {
                Id = currentUser.Id,
                Name = "Private",
                Description = "Private Organization for " + currentUser.Name
            };

            organizations.Insert(0, item);

            return Ok(organizations);
        }

        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();
            
            string userId = User.Identity.GetUserId();
            var user = _userRepository.Find(userId);
            if (user == null)
                return Unauthorized();

            // user must be member
            if (!user.Organizations.Contains(id))
                return Unauthorized();

            var organization = _organizationRepository.Find(id);
            if (organization == null)
                return NotFound();

            return Ok(organization);
        }

        [Route("")]
        public IHttpActionResult Post([FromBody]Organization value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!HasOwnerAccess(value.Id))
                return Unauthorized();

            // make sure owner
            if (value.Owners.Count == 0)
                value.Owners.Add(User.Identity.GetUserId());

            var organization = _organizationRepository.Save(value);
            if (organization == null)
                return NotFound();

            // make sure all owners are members
            foreach (var userId in organization.Owners)
            {
                var user = _userRepository.Find(userId);
                if (user == null)
                    continue;

                if (user.Organizations.Contains(organization.Id))
                    continue;

                user.Organizations.Add(organization.Id);
                _userRepository.Save(user);
            }

            return Ok(organization);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            if (!HasOwnerAccess(id))
                return Unauthorized();

            _organizationRepository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        private bool HasOwnerAccess(string organizationId)
        {
            var organization = _organizationRepository.Find(organizationId);
            if (organization == null)
                return true; // access created organizations

            string userId = User.Identity.GetUserId();

            // user must be owner
            return organization.Owners.Contains(userId);
        }
    }
}