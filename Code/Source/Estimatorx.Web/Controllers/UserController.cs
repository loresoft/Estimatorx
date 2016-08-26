using System;
using System.Web.Mvc;
using Estimatorx.Core.Security;

namespace Estimatorx.Web.Controllers
{
    [Authorize(Roles = RoleNames.Administrators)]
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(string id)
        {
            return View(model: id);
        }

    }
}