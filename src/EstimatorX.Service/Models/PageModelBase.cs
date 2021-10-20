using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Service.Models
{
    public abstract class PageModelBase : PageModel
    {
        protected PageModelBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        protected ILogger Logger { get; }

        protected void ShowAlert(string message, string type = "success")
        {
            TempData["alert.type"] = type;
            TempData["alert.message"] = message;
        }
    }
}
