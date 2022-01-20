using EstimatorX.Shared.Definitions;
using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Stores;

public class UserStore : StoreBase<User>, IServiceScoped
{
    public UserStore(ILoggerFactory loggerFactory) : base(loggerFactory)
    {
    }
}
