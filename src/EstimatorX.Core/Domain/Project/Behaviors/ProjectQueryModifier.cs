using System.Security.Claims;

using Arbiter.CommandQuery.EntityFramework.Pipeline;
using Arbiter.CommandQuery.Extensions;

using EstimatorX.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace EstimatorX.Domain.Behaviors;

[RegisterSingleton(Duplicate = DuplicateStrategy.Append)]
public class ProjectQueryModifier : IQueryModifier<Project>
{
    public int Priority { get; } = 1;

    public ValueTask<IQueryable<Project>> Apply(
        IQueryable<Project> query,
        DbContext context,
        ClaimsPrincipal? principal = null,
        CancellationToken cancellationToken = default)
    {
        var userId = principal?.GetUserId<long>();
        if (userId is null)
            return ValueTask.FromResult(query.Where(_ => false));

        query = query.Where(x => (x.UserId != null && x.UserId == userId.Value)
            || (x.Organization != null && x.Organization.UserOrganizations.Any(o => o.UserId == userId.Value)));

        return ValueTask.FromResult(query);
    }
}
