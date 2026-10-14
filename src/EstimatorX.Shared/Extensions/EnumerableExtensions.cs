namespace EstimatorX.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IEnumerable{T}"/>.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Concatenates the string representations of the elements in a sequence, using the specified delimiter between each element.
    /// </summary>
    /// <typeparam name="T">The type of the elements in <paramref name="values"/>.</typeparam>
    /// <param name="values">The sequence of values to concatenate.</param>
    /// <param name="delimiter">
    /// The string to use as a separator. If <see langword="null"/>, a comma (<c>,</c>) is used. Defaults to a comma.
    /// </param>
    /// <returns>
    /// A string consisting of the elements of <paramref name="values"/> delimited by <paramref name="delimiter"/>,
    /// or <see cref="string.Empty"/> if <paramref name="values"/> has no elements.
    /// </returns>
    /// <remarks>
    /// Each element is converted using its <see cref="object.ToString"/> method; <see langword="null"/> elements are represented as empty strings.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null"/>.</exception>
    /// <example>
    /// <code>
    /// var result = new[] { 1, 2, 3 }.ToDelimitedString("; "); // "1; 2; 3"
    /// </code>
    /// </example>
    public static string ToDelimitedString<T>(this IEnumerable<T?> values, string? delimiter = ",")
        => string.Join(delimiter ?? ",", values);

    /// <summary>
    /// Concatenates the elements of a string sequence, using the specified delimiter between each element.
    /// </summary>
    /// <param name="values">The sequence of strings to concatenate.</param>
    /// <param name="delimiter">
    /// The string to use as a separator. If <see langword="null"/>, a comma (<c>,</c>) is used. Defaults to a comma.
    /// </param>
    /// <returns>
    /// A string consisting of the elements of <paramref name="values"/> delimited by <paramref name="delimiter"/>,
    /// or <see cref="string.Empty"/> if <paramref name="values"/> has no elements.
    /// </returns>
    /// <remarks>
    /// <see langword="null"/> elements are represented as empty strings.
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null"/>.</exception>
    /// <example>
    /// <code>
    /// var result = new[] { "a", "b", "c" }.ToDelimitedString("|"); // "a|b|c"
    /// </code>
    /// </example>
    public static string ToDelimitedString(this IEnumerable<string?> values, string? delimiter = ",")
        => string.Join(delimiter ?? ",", values);
}
