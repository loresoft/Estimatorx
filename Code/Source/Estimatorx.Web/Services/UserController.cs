using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Estimatorx.Core.Query;
using Estimatorx.Core.Security;
using Estimatorx.Data.Mongo.Security;
using Estimatorx.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MongoDB.Driver.Linq;

namespace Estimatorx.Web.Services
{
    [Authorize]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationRepository _organizationRepository;

        private readonly UserManager _userManager;
        private readonly SignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;

        public UserController(
            IUserRepository userRepository,
            IOrganizationRepository organizationRepository,
            UserManager userManager,
            SignInManager signInManager,
            IAuthenticationManager authenticationManager
        )
        {
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
        }

        [HttpGet]
        [Route("Profile")]
        public IHttpActionResult Profile()
        {
            string userId = User.Identity.GetUserId();
            var user = _userRepository.Find(userId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = RoleNames.Administrators)]
        public IHttpActionResult Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var user = _userRepository.Find(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]User value)
        {
            string userId = User.Identity.GetUserId();
            bool isSelf = userId == value.Id;

            if (!isSelf && !User.IsInRole(RoleNames.Administrators))
                return Unauthorized(); // can only save self

            var user = _userRepository.Save(value);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            string userId = User.Identity.GetUserId();
            bool isSelf = userId == id;

            if (!isSelf && !User.IsInRole(RoleNames.Administrators))
                return Unauthorized(); // can only delete self

            _userRepository.Delete(id);
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpGet]
        [Route("Search")]
        public IHttpActionResult Search(string q)
        {
            var users = _userRepository
                .FindAll(u => u.Name.Contains(q) || u.Email.Contains(q))
                .Select(u => new UserModel { Id = u.Id, Name = u.Name, Email = u.Email })
                .OrderBy(u => u.Name)
                .Take(20);

            return Ok(users);
        }

        [HttpGet]
        [Route("Query")]
        [Authorize(Roles = RoleNames.Administrators)]
        public IHttpActionResult Query(int? page = null, int? pageSize = null, string sort = null, bool? descending = null, string search = null, string organization = null)
        {
            var query = _userRepository.All();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Name.ToLower().Contains(search));


            var result = query
                .ToDataResult(config => config
                    .Page(page ?? 1)
                    .PageSize(pageSize ?? 50)
                    .Sort(sort)
                    .Descending(descending ?? false)
                );

            return Ok(result);
        }

        [HttpPost]
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword([FromBody]PasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userManager.AddPasswordAsync(
                User.Identity.GetUserId(),
                model.NewPassword);

            if (result.Succeeded)
                return Ok();

            return IdentityError(result);
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword([FromBody]ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userManager.ChangePasswordAsync(
                User.Identity.GetUserId(),
                model.OldPassword,
                model.NewPassword);

            if (result.Succeeded)
                return Ok();

            return IdentityError(result);
        }


        [HttpPost]
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(ExternalLoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userLoginInfo = new UserLoginInfo(model.LoginProvider, model.ProviderKey);

            var result = await _userManager.RemoveLoginAsync(
                User.Identity.GetUserId(),
                userLoginInfo);

            if (result.Succeeded)
                return Ok();

            return IdentityError(result);
        }


        [HttpGet]
        [Route("OrganizationMembers/{organizationId}")]
        public IHttpActionResult OrganizationMembers(string organizationId)
        {
            var organization = _organizationRepository.Find(organizationId);
            if (organization == null)
                return NotFound();

            var users = _userRepository
                .OrganizationMembers(organizationId)
                .ToList();

            return Ok(users);
        }

        [HttpGet]
        [Route("OrganizationOwners/{organizationId}")]
        public IHttpActionResult OrganizationOwners(string organizationId)
        {
            var organization = _organizationRepository.Find(organizationId);
            if (organization == null)
                return NotFound();

            var users = _userRepository
                .FindAll(organization.Owners)
                .ToList();

            return Ok(users);
        }

        [HttpPost]
        [Route("AddOrganization")]
        public IHttpActionResult AddOrganization(OrganizationUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _userRepository.Find(model.UserId);
            if (user == null)
                return NotFound();

            user.Organizations.Add(model.OrganizationId);

            _userRepository.Save(user);

            return Ok();
        }

        [HttpPost]
        [Route("RemoveOrganization")]
        public IHttpActionResult RemoveOrganization(OrganizationUser model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _userRepository.Find(model.UserId);
            if (user == null)
                return NotFound();

            user.Organizations.Remove(model.OrganizationId);

            _userRepository.Save(user);

            return Ok();
        }


        private IHttpActionResult IdentityError(IdentityResult result)
        {
            if (result == null)
                return InternalServerError();

            if (result.Succeeded)
                return null;

            if (result.Errors != null)
                foreach (string error in result.Errors)
                    ModelState.AddModelError("", error);

            if (ModelState.IsValid)
                return BadRequest();

            return BadRequest(ModelState);
        }
    }
}