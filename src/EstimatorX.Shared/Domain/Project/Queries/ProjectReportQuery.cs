using System.Security.Claims;
using System.Text.Json.Serialization;

using Arbiter.CommandQuery.Commands;

using EstimatorX.Domain.Models;

namespace EstimatorX.Domain.Queries;

[MessagePackObject(true)]
public record ProjectReportQuery : PrincipalCommandBase<ProjectReadModel>
{
    public ProjectReportQuery(ClaimsPrincipal? principal, string securityKey) : base(principal)
    {
        SecurityKey = securityKey;
    }

    [JsonConstructor]
    [SerializationConstructor]
    public ProjectReportQuery(string securityKey) : this(principal: null, securityKey)
    {
    }

    public string SecurityKey { get; }
}
