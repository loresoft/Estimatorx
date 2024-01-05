using System.Text.Json;
using System.Text.Json.Serialization;

namespace EstimatorX.Shared.Models;

public class LogEvent
{
    [JsonIgnore]
    public string RowKey { get; } = Guid.NewGuid().ToString("N");

    [JsonPropertyName("@t")]
    public DateTimeOffset? Timestamp { get; set; }

    [JsonPropertyName("@l")]
    public string Level { get; set; } = "Information";

    [JsonPropertyName("@mt")]
    public string MessageTemplate { get; set; }

    [JsonPropertyName("@m")]
    public string RenderedMessage { get; set; }

    [JsonPropertyName("@x")]
    public string Exception { get; set; }

    [JsonPropertyName("@i")]
    public string EventId { get; set; }

    [JsonPropertyName("@r")]
    public string[] Renderings { get; set; }

    [JsonPropertyName("@tr")]
    public string TraceId { get; set; }

    [JsonPropertyName("@sp")]
    public string SpanId { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement> Data { get; set; }
}
