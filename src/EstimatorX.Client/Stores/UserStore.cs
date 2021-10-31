using EstimatorX.Shared.Models;
using Microsoft.Extensions.Logging;

namespace EstimatorX.Client.Stores;

public class UserStore : StoreBase<UserModel>
{
    public UserStore(ILoggerFactory loggerFactory) : base(loggerFactory)
    {
    }
}
