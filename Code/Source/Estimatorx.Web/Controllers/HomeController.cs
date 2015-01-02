using System;
using System.Web.Mvc;

namespace Estimatorx.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            var i = User.Identity;

            return View();
        }
    }
}
