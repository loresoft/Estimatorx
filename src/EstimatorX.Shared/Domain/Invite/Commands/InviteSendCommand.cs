using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Json.Serialization;

using Arbiter.CommandQuery.Commands;

using Arbiter.CommandQuery.Models;

namespace EstimatorX.Domain.Commands;

[MessagePackObject(true)]
public record InviteSendCommand : EntityIdentifierBase<long, CompleteModel>
{
    public InviteSendCommand(
        ClaimsPrincipal? principal,
        [NotNull] long id,
        string? baseAddress = null)
        : base(principal, id)
    {
        BaseAddress = baseAddress;
    }

    [JsonConstructor]
    [SerializationConstructor]
    public InviteSendCommand(
        [NotNull] long id,
        string? baseAddress = null)
        : this(principal: null, id, baseAddress)
    {
    }

    public string? BaseAddress { get; }
}
