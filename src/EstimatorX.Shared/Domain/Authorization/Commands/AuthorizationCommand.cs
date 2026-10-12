using System.Security.Claims;
using System.Text.Json.Serialization;

using Arbiter.CommandQuery.Commands;

using EstimatorX.Domain.Models;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace EstimatorX.Domain.Commands;

[MessagePackObject(true)]
public record AuthorizationCommand : PrincipalCommandBase<AuthorizationModel>
{
    public AuthorizationCommand(ClaimsPrincipal? principal) : base(principal)
    {
    }

    [JsonConstructor]
    [SerializationConstructor]
    public AuthorizationCommand() : base(principal: null)
    {
    }

    [JsonPropertyName("provider")]
    public string? Provider { get; init; }

    [JsonPropertyName("providerKey")]
    public string? ProviderKey { get; init; }
}
