using System;
using System.Security.Principal;
using System.Web;
using System.Web.Hosting;

namespace Estimator.Core
{
    public static class UserName
    {
        public static string Current()
        {
            if (!HostingEnvironment.IsHosted)
                return Environment.UserName;

            IPrincipal currentUser = null;
            HttpContext current = HttpContext.Current;
            if (current != null)
                currentUser = current.User;

            if ((currentUser != null))
                return currentUser.Identity.Name;

            return Environment.UserName;
        }
    }
}