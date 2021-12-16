using EstimatorX.Shared.Models;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Client.Stores;

public class OrganizationStore : StoreBase<Organization>
{
    public OrganizationStore(ILoggerFactory loggerFactory) : base(loggerFactory)
    {
    }
}
