using Arbiter.CommandQuery.Definitions;
using Arbiter.CommandQuery.EntityFramework.Pipeline;
using Arbiter.CommandQuery.Extensions;
using Arbiter.CommandQuery.Notifications;

using EstimatorX.Data.Entities;

using Microsoft.EntityFrameworkCore;

namespace EstimatorX.Domain.Behaviors;

[RegisterSingleton(Duplicate = DuplicateStrategy.Append)]
public class OrganizationEntityModifier : IEntityModifier<Organization>
{
    public int Priority { get; } = 1;

    public async ValueTask Apply(
        Organization entity,
        DbContext context,
        EntityChangeOperation operation,
        IRequestPrincipal? principal = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (operation != EntityChangeOperation.Created)
            return;

        // assign the creating user as owner of the new organization

        var userId = principal?.Principal.GetUserId<long>();
        if (userId is null)
            return;

        if (entity.UserOrganizations.Any(o => o.UserId == userId.Value))
            return;

        var owner = new UserOrganization
        {
            UserId = userId.Value,
            IsOwner = true,
            Created = principal?.Activated ?? DateTimeOffset.UtcNow,
            CreatedBy = principal?.ActivatedBy,
            Updated = principal?.Activated ?? DateTimeOffset.UtcNow,
            UpdatedBy = principal?.ActivatedBy,
        };

        entity.UserOrganizations.Add(owner);
    }
}
