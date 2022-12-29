using EstimatorX.Client.Repositories;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Stores;

[RegisterScoped]
public class TemplateStore : StoreEditBase<Template, TemplateRepository>
{
    public TemplateStore(ILoggerFactory loggerFactory, TemplateRepository repository) : base(loggerFactory, repository)
    {
    }
}
