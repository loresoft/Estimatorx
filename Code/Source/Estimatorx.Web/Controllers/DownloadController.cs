using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Estimatorx.Core;
using Estimatorx.Core.Providers;
using Estimatorx.Core.Security;
using Microsoft.AspNet.Identity;

namespace Estimatorx.Web.Controllers
{
    [Authorize]
    public class DownloadController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public DownloadController(IProjectRepository projectRepository, IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public ActionResult Report(string id)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();

            Project project;
            if (!HasAccess(id, out project))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            if (project == null)
                return HttpNotFound();

            var creator = new DocumentCreator();
            var buffer = creator.CreatePdf(project);

            return File(buffer, "application/pdf", project.Name + ".pdf");
        }

        [AllowAnonymous]
        [Route("Download/Share/{id}/{key}")]
        public ActionResult Share(string id, string key)
        {
            if (string.IsNullOrEmpty(id))
                return HttpNotFound();
            if (string.IsNullOrEmpty(key))
                return HttpNotFound();

            var project = _projectRepository.Find(id);
            if (project == null)
                return HttpNotFound();

            if (project.SecurityKey != key)
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            var creator = new DocumentCreator();
            var buffer = creator.CreatePdf(project);

            return File(buffer, "application/pdf", project.Name + ".pdf");
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