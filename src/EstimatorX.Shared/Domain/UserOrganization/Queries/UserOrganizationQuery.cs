using System.Security.Claims;
using System.Text.Json.Serialization;

using Arbiter.CommandQuery.Commands;

using EstimatorX.Domain.Models;

namespace EstimatorX.Domain.Queries;

[MessagePackObject(true)]
public record UserOrganizationQuery : CacheableQueryBase<UserOrganizationReadModel>
{
    public UserOrganizationQuery(ClaimsPrincipal? principal, long userId, long organizationId) : base(principal)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nameof(userId));

        UserId = userId;
        OrganizationId = organizationId;
    }

    [JsonConstructor]
    [SerializationConstructor]
    public UserOrganizationQuery(long userId, long organizationId) : this(principal: null, userId, organizationId)
    {
    }

    public long UserId { get; }

    public long OrganizationId { get; }

    public override string GetCacheKey()
        => $"UserOrganization:{UserId}:{OrganizationId}";
}
