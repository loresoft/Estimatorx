using System;
using System.Web.Mvc;
using Estimatorx.Web.Filters;

namespace Estimatorx.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogErrorAttribute());
        }
    }
}
