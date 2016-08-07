using System;
using System.Web.Mvc;

namespace Estimatorx.Web.Controllers
{
    [Authorize(Roles = "administrators")]
    public class LoggingController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
