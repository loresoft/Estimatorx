using System;
using System.Web.Mvc;

namespace Estimatorx.Web.Controllers
{
    [Authorize(Roles = "administrators")]
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