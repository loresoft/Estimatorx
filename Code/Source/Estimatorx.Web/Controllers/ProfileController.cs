using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Estimatorx.Core.Security;
using Estimatorx.Web.Security;
using Microsoft.AspNet.Identity;

namespace Estimatorx.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        [RequireHttps]
        public ActionResult Index()
        {
            return View();
        }


    }
}