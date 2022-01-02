namespace EstimatorX.Shared.Extensions;

public static class TypeExtensions
{
    /// <summary>
    /// Gets the underlying type dealing with <see cref="T:Nullable`1"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>Returns a type dealing with <see cref="T:Nullable`1"/>.</returns>
    public static Type GetUnderlyingType(this Type type)
    {
        if (type == null)
            throw new ArgumentNullException("type");

        Type t = type;
        bool isNullable = t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(Nullable<>));
        if (isNullable)
            return Nullable.GetUnderlyingType(t);

        return t;
    }

    /// <summary>
    /// Determines whether the specified <paramref name="type"/> can be null.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified <paramref name="type"/> can be null; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullable(this Type type)
    {
        if (!type.IsValueType)
            return true;

        return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
    }

    /// <summary>
    /// Determines whether the specified type implements an interface.
    /// </summary>
    /// <typeparam name="TInterface">The type of the interface.</typeparam>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if type implements the interface; otherwise <c>false</c></returns>
    /// <exception cref="InvalidOperationException">Only interfaces can be implemented.</exception>
    public static bool Implements<TInterface>(this Type type)
        where TInterface : class
    {
        var interfaceType = typeof(TInterface);

        if (!interfaceType.IsInterface)
            throw new InvalidOperationException("Only interfaces can be implemented.");

        return interfaceType.IsAssignableFrom(type);
    }
}
