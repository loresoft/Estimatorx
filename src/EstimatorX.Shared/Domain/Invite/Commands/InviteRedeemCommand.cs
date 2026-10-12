using System.Security.Claims;
using System.Text.Json.Serialization;

using Arbiter.CommandQuery.Commands;
using Arbiter.CommandQuery.Models;
using Arbiter.Mediation;

namespace EstimatorX.Domain.Commands;

[MessagePackObject(true)]
public record InviteRedeemCommand : PrincipalCommandBase<CompleteModel>
{
    public InviteRedeemCommand(ClaimsPrincipal? principal, string token)
        : base(principal)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);
        Token = token;
    }

    [JsonConstructor]
    [SerializationConstructor]
    public InviteRedeemCommand(string token)
        : this(principal: null, token)
    {
    }

    public string Token { get; }
}
