using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Estimatorx.Core;
using Estimatorx.Core.Security;
using Estimatorx.Core.Query;
using Estimatorx.Data.Mongo;
using Estimatorx.Data.Mongo.Security;
using Microsoft.AspNet.Identity;

namespace Estimatorx.Web.Services
{
    [Authorize]
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

        public IHttpActionResult Get(int? page = null, int? pageSize = null, string sort = null, bool? descending = null)
        {
            var currentUser = _userRepository.Find(User.Identity.GetUserId());
            if (currentUser == null)
                return NotFound();

            // find orgs for current user only
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

        public IHttpActionResult Post([FromBody]Organization value)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!HasOwnerAccess(value.Id))
                return Unauthorized();
            
            var organization = _organizationRepository.Save(value);
            if (organization == null)
                return NotFound();

            return Ok(organization);
        }

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