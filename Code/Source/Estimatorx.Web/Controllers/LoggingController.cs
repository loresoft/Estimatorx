using System;
using System.Web.Mvc;
using Estimatorx.Core.Security;

namespace Estimatorx.Web.Controllers
{
    [Authorize(Roles = RoleNames.Administrators)]
    public class LoggingController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
