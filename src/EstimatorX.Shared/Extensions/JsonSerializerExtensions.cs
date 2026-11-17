using System.Text.Json;
using System.Text.Json.Serialization;

namespace EstimatorX.Extensions;

public static class JsonSerializerExtensions
{
    public static JsonSerializerOptions AddDomainOptions(this JsonSerializerOptions options)
    {
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;

        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

        return options;
    }
}
