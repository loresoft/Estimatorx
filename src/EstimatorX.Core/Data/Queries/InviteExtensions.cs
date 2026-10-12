using EstimatorX.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace EstimatorX.Data.Queries;

/// <summary>
/// Provides extension methods for querying and manipulating Invite entities in the database.
/// </summary>
public static class InviteExtensions
{
    /// <summary>
    /// Removes invites that expired more than the grace period ago.
    /// </summary>
    /// <returns>The number of invites removed.</returns>
    public static Task<int> RemoveExpiredInvites(
        this IQueryable<Invite> query,
        int graceDays = 30,
        CancellationToken cancellationToken = default)
    {
        var gracePeriod = DateTimeOffset.UtcNow.AddDays(-graceDays);

        return query
            .Where(i => i.Expiration < gracePeriod)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
