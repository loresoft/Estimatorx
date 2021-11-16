using EstimatorX.Shared.Models;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Client.Stores;

public class OrganizationStore : StoreBase<OrganizationModel>
{
    public OrganizationStore(ILoggerFactory loggerFactory) : base(loggerFactory)
    {
    }
}
