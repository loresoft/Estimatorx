using System;
using System.Web.Mvc;
using MongoDB.Bson;

namespace Estimatorx.Web.Controllers
{
    [Authorize]
    public class TemplateController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View("Edit");
        }


        public ActionResult Edit(string id)
        {
            return View(model: id);
        }

    }
}