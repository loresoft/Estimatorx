using System.Text;

namespace EstimatorX.Shared.Extensions;

public static class EnumerableExtensions
{
    public static string ToDelimitedString<T>(this IEnumerable<T> values)
    {
        return ToDelimitedString<T>(values, ",");
    }

    public static string ToDelimitedString<T>(this IEnumerable<T> values, string delimiter)
    {
        if (values is null)
            return null;

        var sb = new StringBuilder();
        foreach (var i in values)
        {
            if (sb.Length > 0)
                sb.Append(delimiter ?? ",");
            sb.Append(i.ToString());
        }

        return sb.ToString();
    }

    public static string ToDelimitedString(this IEnumerable<string> values)
    {
        return ToDelimitedString(values, ",");
    }

    public static string ToDelimitedString(this IEnumerable<string> values, string delimiter)
    {
        return ToDelimitedString(values, delimiter, null);
    }

    public static string ToDelimitedString(this IEnumerable<string> values, string delimiter, Func<string, string> escapeDelimiter)
    {
        if (values is null)
            return null;

        var sb = new StringBuilder();
        foreach (var value in values)
        {
            if (sb.Length > 0)
                sb.Append(delimiter);

            var v = escapeDelimiter != null
                ? escapeDelimiter(value ?? string.Empty)
                : value ?? string.Empty;

            sb.Append(v);
        }

        return sb.ToString();
    }

    public static async Task<T> FirstOrDefaultAsync<T>(this IAsyncEnumerable<T> enumerable)
    {
        await foreach (var item in enumerable)
            return item;

        return default;
    }
}
