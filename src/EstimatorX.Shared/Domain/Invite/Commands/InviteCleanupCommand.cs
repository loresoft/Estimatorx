using System.Text.Json.Serialization;

using Arbiter.CommandQuery.Models;
using Arbiter.Mediation;

namespace EstimatorX.Domain.Commands;

[MessagePackObject(true)]
public record InviteCleanupCommand : IRequest<CompleteModel>
{
    [JsonConstructor]
    [SerializationConstructor]
    public InviteCleanupCommand()
    {
    }
}
