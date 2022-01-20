
using EstimatorX.Client.Services;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Repositories;

public class TemplateRepository : RepositorySearchBase<Template, TemplateSummary>, IServiceScoped
{
    public TemplateRepository(GatewayClient gateway) : base(gateway)
    {
    }

    protected override string GetBasePath()
    {
        return "/api/template";
    }
}
