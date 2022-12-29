using EstimatorX.Shared.Models;

namespace EstimatorX.Client.Stores;

[RegisterScoped]
public class UserStore : StoreBase<User>
{
    public UserStore(ILoggerFactory loggerFactory) : base(loggerFactory)
    {
    }
}
