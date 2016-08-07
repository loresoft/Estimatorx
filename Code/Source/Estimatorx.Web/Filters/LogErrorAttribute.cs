using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog.Fluent;

namespace Estimatorx.Web.Filters
{
    public class LogErrorAttribute : HandleErrorAttribute
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public override void OnException(ExceptionContext filterContext)
        {
            // let base try to handle
            base.OnException(filterContext);

            // let error bubble up to global logging
            if (!filterContext.ExceptionHandled)
                return;

            Exception exception = filterContext.Exception;

            _logger.Error()
                .Message("Http Application Error: " + exception.GetBaseException().Message)
                .Exception(exception)
                .Property("RequestUri", filterContext.RequestContext?.HttpContext?.Request?.Url?.ToString())
                .Write();
        }
    }
}