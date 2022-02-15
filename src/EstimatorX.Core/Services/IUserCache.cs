
using EstimatorX.Core.Repositories;
using EstimatorX.Shared.Models;

namespace EstimatorX.Core.Services;

public interface IUserCache
{
    IUserRepository Repository { get; }

    Task<User> Get(string userId, CancellationToken cancellationToken = default);

    void Clear(string userId);
}
