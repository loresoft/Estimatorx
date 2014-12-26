using System;
using System.Web.Mvc;
using MongoDB.Bson;

namespace Estimatorx.Web.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var routeValues = new { id = ObjectId.GenerateNewId().ToString() };
            return RedirectToAction("Edit", routeValues);
        }

        public ActionResult Edit(string id)
        {
            return View(model: id);
        }

    }
}