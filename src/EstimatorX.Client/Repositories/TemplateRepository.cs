using System.Net.Http;

using EstimatorX.Client.Services;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Repositories;

public class TemplateRepository : RepositoryBase<TemplateModel>
{
    public TemplateRepository(GatewayClient gateway) : base(gateway)
    {
    }

    protected override string GetBasePath()
    {
        return "/api/Template";
    }
}
