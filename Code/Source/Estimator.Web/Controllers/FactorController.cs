using System;
using System.Web.Mvc;

namespace Estimatorx.Web.Controllers
{
    public class FactorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var routeValues = new { id = Guid.NewGuid() };
            return RedirectToAction("Edit", routeValues);
        }


        public ActionResult Edit(Guid id)
        {
            return View(id);
        }

    }
}