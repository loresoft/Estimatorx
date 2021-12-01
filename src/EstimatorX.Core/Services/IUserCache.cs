
using EstimatorX.Core.Entities;

namespace EstimatorX.Core.Services;

public interface IUserCache
{
    Task<User> GetCachedUser(string userId, CancellationToken cancellationToken = default);

    void RemoveCachedUser(string userId);
}