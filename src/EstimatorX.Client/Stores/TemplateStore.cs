using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Models;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Client.Stores;

public class TemplateStore : StoreBase<Template>, IScopedService
{
    public TemplateStore(ILoggerFactory loggerFactory) : base(loggerFactory)
    {
    }
}
