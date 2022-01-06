using EstimatorX.Client.Repositories;
using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Stores;

public class OrganizationStore : StoreEditBase<Organization, OrganizationRepository>, IScopedService
{
    public OrganizationStore(ILoggerFactory loggerFactory, OrganizationRepository organizationRepository)
        : base(loggerFactory, organizationRepository)
    {
    }
}
