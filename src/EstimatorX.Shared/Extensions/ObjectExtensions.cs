using System.Text.Json;

namespace EstimatorX.Shared.Extensions;

public static class ObjectExtensions
{
    public static T JsonClone<T>(this T instance, JsonSerializerOptions serializerOptions = null)
    {
        if (instance == null)
            return instance;

        serializerOptions ??= new JsonSerializerOptions(JsonSerializerDefaults.Web);

        var json = JsonSerializer.Serialize(instance, serializerOptions);
        return JsonSerializer.Deserialize<T>(json, serializerOptions);
    }
}
