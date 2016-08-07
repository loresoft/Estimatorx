using System;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Estimatorx.Web.Filters;

namespace Estimatorx.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Services.Add(typeof(IExceptionLogger), new GlobalExceptionLogger());
        }
    }
}
