
using EstimatorX.Core.Repositories;
using EstimatorX.Shared.Definitions;

using Microsoft.Extensions.Caching.Memory;

namespace EstimatorX.Core.Services;

public class UserCache : IUserCache, ISingletonService
{
    private readonly IUserRepository _userRepository;
    private readonly IMemoryCache _memoryCache;

    public UserCache(IMemoryCache memoryCache, IUserRepository userRepository)
    {
        _memoryCache = memoryCache;
        _userRepository = userRepository;
    }

    public async Task<Entities.User> Get(string userId, CancellationToken cancellationToken = default)
    {
        var key = CreateKey(userId);

        return await _memoryCache.GetOrCreateAsync(key, (cacheEntry) =>
        {
            cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(30);
            return _userRepository.FindAsync(userId, cancellationToken: cancellationToken);
        });
    }

    public void Clear(string userId)
    {
        var key = CreateKey(userId);
        _memoryCache.Remove(key);
    }

    public static string CreateKey(string userId)
    {
        return $"global::user:{userId}";
    }
}
