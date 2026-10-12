using System.Security.Claims;

using Arbiter.CommandQuery.EntityFramework.Pipeline;
using Arbiter.CommandQuery.Extensions;

using EstimatorX.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace EstimatorX.Domain.Behaviors;

[RegisterSingleton(Duplicate = DuplicateStrategy.Append)]
public class OrganizationQueryModifier : IQueryModifier<Organization>
{
    public int Priority { get; } = 1;

    public ValueTask<IQueryable<Organization>> Apply(
        IQueryable<Organization> query,
        DbContext context,
        ClaimsPrincipal? principal = null,
        CancellationToken cancellationToken = default)
    {
        var userId = principal?.GetUserId<long>();
        if (userId is null)
            return ValueTask.FromResult(query.Where(_ => false));

        query = query.Where(x => x.UserOrganizations.Any(o => o.UserId == userId.Value));

        return ValueTask.FromResult(query);
    }
}
