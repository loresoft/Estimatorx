using System;
using System.Linq;
using System.Net.Http;

using EstimatorX.Client.Services;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Repositories;

public class OrganizationRepository : RepositoryBase<OrganizationModel>
{
    public OrganizationRepository(GatewayClient gateway) : base(gateway)
    {
    }

    protected override string GetBasePath()
    {
        return "/api/Organization";
    }
}
