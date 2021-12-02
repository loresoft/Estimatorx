
using EstimatorX.Core.Entities;

namespace EstimatorX.Core.Services;

public interface IUserCache
{
    Task<User> Get(string userId, CancellationToken cancellationToken = default);

    void Clear(string userId);
}
