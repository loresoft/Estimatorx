
using EstimatorX.Client.Services;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Repositories;

public class TemplateRepository : RepositorySearchBase<Template, TemplateSummary>
{
    public TemplateRepository(GatewayClient gateway) : base(gateway)
    {
    }

    protected override string GetBasePath()
    {
        return "/api/template";
    }
}
