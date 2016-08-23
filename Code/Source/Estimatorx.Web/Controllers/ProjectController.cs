using System;
using System.Web.Mvc;
using Estimatorx.Web.Models;
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
            return View("Edit");
        }

        public ActionResult Edit(string id)
        {
            return View(model: id);
        }

        public ActionResult Report(string id)
        {
            return View(model: id);
        }

        [AllowAnonymous]
        [Route("Project/Share/{id}/{key}")]
        public ActionResult Share(string id, string key)
        {
            var model = new ReportModel {Id = id, Key = key};
            return View(model);
        }
    }
}