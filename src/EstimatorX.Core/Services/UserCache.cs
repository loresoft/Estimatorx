
using EstimatorX.Core.Repositories;

using Microsoft.Extensions.Caching.Memory;

namespace EstimatorX.Core.Services;

public class UserCache : IUserCache
{
    private readonly IUserRepository _userRepository;
    private readonly IMemoryCache _memoryCache;

    public UserCache(IMemoryCache memoryCache, IUserRepository userRepository)
    {
        _memoryCache = memoryCache;
        _userRepository = userRepository;
    }

    public async Task<Entities.User> GetCachedUser(string userId)
    {
        var key = CreateKey(userId);

        return await _memoryCache.GetOrCreateAsync(key, (cacheEntry) =>
        {
            cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(30);
            return _userRepository.FindAsync(userId);
        });
    }

    public void RemoveCachedUser(string userId)
    {
        var key = CreateKey(userId);
        _memoryCache.Remove(key);
    }

    protected virtual string CreateKey(string userId)
    {
        return $"global::user:{userId}";
    }
}
