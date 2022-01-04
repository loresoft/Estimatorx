using System.Text.Json;

namespace EstimatorX.Shared.Extensions;

public static class ObjectExtensions
{
    public static T Clone<T>(this T instance)
    {
        var buffer = JsonSerializer.SerializeToUtf8Bytes(instance);

        return JsonSerializer.Deserialize<T>(buffer);
    }
}
