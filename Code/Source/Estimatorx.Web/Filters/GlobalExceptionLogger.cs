using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using NLog.Fluent;

namespace Estimatorx.Web.Filters
{
    public class GlobalExceptionLogger : ExceptionLogger
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public override void Log(ExceptionLoggerContext context)
        {
            Exception exception = context.Exception;

            _logger.Error()
                .Message("Http Application Error: " + exception.GetBaseException().Message)
                .Exception(exception)
                .Property("RequestUri", context.Request?.RequestUri?.ToString())
                .Write();
        }
    }
}
