using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Estimatorx.Core.Security;
using Estimatorx.Data.Mongo.Security;
using Estimatorx.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Estimatorx.Web.Services
{
    [Authorize]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        private readonly IUserRepository _repository;
        private readonly UserManager _userManager;
        private readonly SignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;

        public UserController(
            IUserRepository repository,
            UserManager userManager,
            SignInManager signInManager,
            IAuthenticationManager authenticationManager)
        {
            _repository = repository;
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
        }


        public IEnumerable<User> Get()
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

        public IHttpActionResult Post([FromBody]User value)
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