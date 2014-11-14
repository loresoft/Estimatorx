using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Estimator.Web.Controllers
{
    public class EstimateController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(Guid? id)
        {
            return View(id);
        }

    }
}