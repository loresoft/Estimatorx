using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Estimator.Web.Controllers
{
    [Authorize]
    public class EstimateController : Controller
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