using EstimatorX.Shared.Models;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Client.Stores;

public class UserStore : StoreBase<User>
{
    public UserStore(ILoggerFactory loggerFactory) : base(loggerFactory)
    {
    }
}
