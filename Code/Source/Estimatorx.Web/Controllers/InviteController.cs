using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Estimatorx.Core.Security;
using Microsoft.AspNet.Identity;

namespace Estimatorx.Web.Controllers
{
    [Authorize]
    public class InviteController : Controller
    {
        private readonly IInviteRepository _inviteRepository;
        private readonly IUserRepository _userRepository;

        public InviteController(
            IInviteRepository inviteRepository,
            IUserRepository userRepository)
        {
            _inviteRepository = inviteRepository;
            _userRepository = userRepository;
        }

        public ActionResult Index(string id, string key)
        {
            string userId = User.Identity.GetUserId();
            var user = _userRepository.Find(userId);
            if (user == null)
                return new HttpNotFoundResult();

            var invite = _inviteRepository.Find(id);
            if (invite == null || invite.SecurityKey != key)
                return new HttpNotFoundResult();

            // add user to organization
            user.Organizations.Add(invite.OrganizationId);
            _userRepository.Save(user);

            // delete invite
            _inviteRepository.Delete(invite);

            return RedirectToAction("Index", "Organization");
        }
    }
}