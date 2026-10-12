using System.Security.Claims;
using System.Text.Json.Serialization;

using Arbiter.CommandQuery.Commands;

using Arbiter.CommandQuery.Models;

namespace EstimatorX.Domain.Commands;

[MessagePackObject(true)]
public record UserOrganizationDeleteCommand : PrincipalCommandBase<CompleteModel>
{
    public UserOrganizationDeleteCommand(ClaimsPrincipal? principal, long userId, long organizationId)
        : base(principal)
    {
        UserId = userId;
        OrganizationId = organizationId;
    }

    [JsonConstructor]
    [SerializationConstructor]
    public UserOrganizationDeleteCommand(long userId, long organizationId)
        : this(principal: null, userId, organizationId)
    {
    }

    public long UserId { get; }

    public long OrganizationId { get; }
}
