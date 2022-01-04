
using EstimatorX.Client.Services;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Repositories;

public class OrganizationRepository : RepositorySearchBase<Organization, OrganizationSummary>
{
    public OrganizationRepository(GatewayClient gateway) : base(gateway)
    {
    }

    protected override string GetBasePath()
    {
        return "/api/organization";
    }
}
