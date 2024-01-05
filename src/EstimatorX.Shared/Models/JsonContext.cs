using System.Text.Json.Serialization;

using Json.Patch;

namespace EstimatorX.Shared.Models;

[JsonSourceGenerationOptions(
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(EffortLevel))]
[JsonSerializable(typeof(EpicEstimate))]
[JsonSerializable(typeof(EstimateMultiplier))]
[JsonSerializable(typeof(FeatureEstimate))]
[JsonSerializable(typeof(IdentifierName))]
[JsonSerializable(typeof(Invite))]
[JsonSerializable(typeof(InviteSummary))]
[JsonSerializable(typeof(Organization))]
[JsonSerializable(typeof(OrganizationMember))]
[JsonSerializable(typeof(OrganizationSummary))]
[JsonSerializable(typeof(Project))]
[JsonSerializable(typeof(ProjectOverhead))]
[JsonSerializable(typeof(ProjectSettings))]
[JsonSerializable(typeof(ProjectSummary))]
[JsonSerializable(typeof(QueryRequest))]
[JsonSerializable(typeof(RiskLevel))]
[JsonSerializable(typeof(StoryEstimate))]
[JsonSerializable(typeof(Template))]
[JsonSerializable(typeof(TemplateSummary))]
[JsonSerializable(typeof(User))]
[JsonSerializable(typeof(UserProfile))]
[JsonSerializable(typeof(UserSummary))]
[JsonSerializable(typeof(QueryResult<InviteSummary>))]
[JsonSerializable(typeof(QueryResult<OrganizationSummary>))]
[JsonSerializable(typeof(QueryResult<ProjectSummary>))]
[JsonSerializable(typeof(QueryResult<TemplateSummary>))]
[JsonSerializable(typeof(QueryResult<UserSummary>))]
[JsonSerializable(typeof(JsonPatch))]
[JsonSerializable(typeof(LogEvent))]
[JsonSerializable(typeof(LogEventRequest))]
[JsonSerializable(typeof(LogEventResult))]
public partial class JsonContext : JsonSerializerContext
{
}
