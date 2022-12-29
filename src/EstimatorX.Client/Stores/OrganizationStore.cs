using EstimatorX.Client.Repositories;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Stores;

[RegisterScoped]
public class OrganizationStore : StoreEditBase<Organization, OrganizationRepository>
{
    public OrganizationStore(ILoggerFactory loggerFactory, OrganizationRepository organizationRepository)
        : base(loggerFactory, organizationRepository)
    {
    }
}
