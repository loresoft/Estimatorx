
using EstimatorX.Shared.Extensions;

namespace EstimatorX.Shared.Models;

public static class CosmosKey
{
    public static bool TryDecode(string cosmosKey, out string id, out string partitionKey)
    {
        if (string.IsNullOrWhiteSpace(cosmosKey))
        {
            id = null;
            partitionKey = default;
            return false;
        }

        var parts = cosmosKey.Split('~');
        if (parts.Length == 1)
        {
            id = parts[0];
            partitionKey = default;
            return id.HasValue();
        }
        if (parts.Length >= 2)
        {
            id = parts[0];
            partitionKey = parts[1];
            return id.HasValue();
        }

        id = null;
        partitionKey = default;
        return false;
    }


    public static string Encode(string id, string partitionKey)
    {
        if (id.IsNullOrWhiteSpace())
            throw new ArgumentNullException(nameof(id));

        return partitionKey.HasValue() ? $"{id}~{partitionKey}" : id;
    }
}
