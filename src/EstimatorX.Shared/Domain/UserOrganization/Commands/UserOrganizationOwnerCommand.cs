using System.Security.Claims;
using System.Text.Json.Serialization;

using Arbiter.CommandQuery.Commands;

using Arbiter.CommandQuery.Models;

namespace EstimatorX.Domain.Commands;

[MessagePackObject(true)]
public record UserOrganizationOwnerCommand : PrincipalCommandBase<CompleteModel>
{
    public UserOrganizationOwnerCommand(ClaimsPrincipal? principal, long userId, long organizationId, bool isOwner)
        : base(principal)
    {
        UserId = userId;
        OrganizationId = organizationId;
        IsOwner = isOwner;
    }

    [JsonConstructor]
    [SerializationConstructor]
    public UserOrganizationOwnerCommand(long userId, long organizationId, bool isOwner)
        : this(principal: null, userId, organizationId, isOwner)
    {
    }

    public long UserId { get; }

    public long OrganizationId { get; }

    public bool IsOwner { get; }
}
