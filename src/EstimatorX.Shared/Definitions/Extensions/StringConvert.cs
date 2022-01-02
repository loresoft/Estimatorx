using System.Globalization;

namespace EstimatorX.Shared.Extensions;

/// <summary>
/// Converts a string data type to another base data type using a safe conversion method.
/// </summary>
public static class StringConvert
{
    /// <summary>
    /// Converts the specified string representation of a logical value to its Boolean equivalent.
    /// </summary>
    /// <param name="value">A string that contains the value of either <see cref="F:System.Boolean.TrueString"/> or <see cref="F:System.Boolean.FalseString"/>.</param>
    /// <returns>
    /// true if <paramref name="value"/> equals <see cref="F:System.Boolean.TrueString"/>, or false if <paramref name="value"/> equals <see cref="F:System.Boolean.FalseString"/> or null.
    /// </returns>
    public static bool? ToBoolean(this string value)
    {
        if (value == null)
            return null;

        if (bool.TryParse(value, out var result))
            return result;

        string v = value.Trim();

        if (string.Equals(v, "t", StringComparison.OrdinalIgnoreCase)
            || string.Equals(v, "true", StringComparison.OrdinalIgnoreCase)
            || string.Equals(v, "y", StringComparison.OrdinalIgnoreCase)
            || string.Equals(v, "yes", StringComparison.OrdinalIgnoreCase)
            || string.Equals(v, "1", StringComparison.OrdinalIgnoreCase)
            || string.Equals(v, "x", StringComparison.OrdinalIgnoreCase)
            || string.Equals(v, "on", StringComparison.OrdinalIgnoreCase))
            return true;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent 8-bit unsigned integer.
    /// </summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <returns>
    /// An 8-bit unsigned integer that is equivalent to <paramref name="value"/>, or zero if <paramref name="value"/> is null.
    /// </returns>
    public static byte? ToByte(this string value)
    {
        if (value == null)
            return null;

        if (byte.TryParse(value, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent 8-bit unsigned integer, using specified culture-specific formatting information.
    /// </summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>
    /// An 8-bit unsigned integer that is equivalent to <paramref name="value"/>, or zero if <paramref name="value"/> is null.
    /// </returns>
    public static byte? ToByte(this string value, IFormatProvider provider)
    {
        if (value == null)
            return null;

        if (byte.TryParse(value, NumberStyles.Integer, provider, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a date and time to an equivalent date and time value.
    /// </summary>
    /// <param name="value">The string representation of a date and time.</param>
    /// <returns>
    /// The date and time equivalent of the value of <paramref name="value"/>, or the date and time equivalent of <see cref="F:System.DateTime.MinValue"/> if <paramref name="value"/> is null.
    /// </returns>
    public static DateTime? ToDateTime(this string value)
    {
        if (value == null)
            return null;

        if (DateTime.TryParse(value, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent date and time, using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="value">A string that contains a date and time to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>
    /// The date and time equivalent of the value of <paramref name="value"/>, or the date and time equivalent of <see cref="F:System.DateTime.MinValue"/> if <paramref name="value"/> is null.
    /// </returns>
    public static DateTime? ToDateTime(this string value, IFormatProvider provider)
    {
        if (value == null)
            return null;

        if (DateTime.TryParse(value, provider, DateTimeStyles.None, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent decimal number.
    /// </summary>
    /// <param name="value">A string that contains a number to convert.</param>
    /// <returns>
    /// A decimal number that is equivalent to the number in <paramref name="value"/>, or 0 (zero) if <paramref name="value"/> is null.
    /// </returns>
    public static decimal? ToDecimal(this string value)
    {
        if (value == null)
            return null;

        if (decimal.TryParse(value, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent decimal number, using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="value">A string that contains a number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>
    /// A decimal number that is equivalent to the number in <paramref name="value"/>, or 0 (zero) if <paramref name="value"/> is null.
    /// </returns>
    public static decimal? ToDecimal(this string value, IFormatProvider provider)
    {
        if (value == null)
            return null;

        if (decimal.TryParse(value, NumberStyles.Number, provider, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent double-precision floating-point number.
    /// </summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <returns>
    /// A double-precision floating-point number that is equivalent to the number in <paramref name="value"/>, or 0 (zero) if <paramref name="value"/> is null.
    /// </returns>
    public static double? ToDouble(this string value)
    {
        if (value == null)
            return null;

        if (double.TryParse(value, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent double-precision floating-point number, using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>
    /// A double-precision floating-point number that is equivalent to the number in <paramref name="value"/>, or 0 (zero) if <paramref name="value"/> is null.
    /// </returns>
    public static double? ToDouble(this string value, IFormatProvider provider)
    {
        if (value == null)
            return null;

        if (double.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, provider, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent 16-bit signed integer.
    /// </summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <returns>
    /// A 16-bit signed integer that is equivalent to the number in <paramref name="value"/>, or 0 (zero) if <paramref name="value"/> is null.
    /// </returns>
    public static short? ToInt16(this string value)
    {
        if (value == null)
            return null;

        if (short.TryParse(value, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent 16-bit signed integer, using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>
    /// A 16-bit signed integer that is equivalent to the number in <paramref name="value"/>, or 0 (zero) if <paramref name="value"/> is null.
    /// </returns>
    public static short? ToInt16(this string value, IFormatProvider provider)
    {
        if (value == null)
            return null;

        if (short.TryParse(value, NumberStyles.Integer, provider, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent 32-bit signed integer.
    /// </summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <returns>
    /// A 32-bit signed integer that is equivalent to the number in <paramref name="value"/>, or 0 (zero) if <paramref name="value"/> is null.
    /// </returns>
    public static int? ToInt32(this string value)
    {
        if (value == null)
            return null;

        if (int.TryParse(value, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent 32-bit signed integer, using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>
    /// A 32-bit signed integer that is equivalent to the number in <paramref name="value"/>, or 0 (zero) if <paramref name="value"/> is null.
    /// </returns>
    public static int? ToInt32(this string value, IFormatProvider provider)
    {
        if (value == null)
            return null;

        if (int.TryParse(value, NumberStyles.Integer, provider, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent 64-bit signed integer.
    /// </summary>
    /// <param name="value">A string that contains a number to convert.</param>
    /// <returns>
    /// A 64-bit signed integer that is equivalent to the number in <paramref name="value"/>, or 0 (zero) if <paramref name="value"/> is null.
    /// </returns>
    public static long? ToInt64(this string value)
    {
        if (value == null)
            return null;

        if (long.TryParse(value, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent 64-bit signed integer, using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>
    /// A 64-bit signed integer that is equivalent to the number in <paramref name="value"/>, or 0 (zero) if <paramref name="value"/> is null.
    /// </returns>
    public static long? ToInt64(this string value, IFormatProvider provider)
    {
        if (value == null)
            return null;

        if (long.TryParse(value, NumberStyles.Integer, provider, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent single-precision floating-point number.
    /// </summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <returns>
    /// A single-precision floating-point number that is equivalent to the number in <paramref name="value"/>, or 0 (zero) if <paramref name="value"/> is null.
    /// </returns>
    public static float? ToSingle(this string value)
    {
        if (value == null)
            return null;

        if (float.TryParse(value, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent single-precision floating-point number, using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>
    /// A single-precision floating-point number that is equivalent to the number in <paramref name="value"/>, or 0 (zero) if <paramref name="value"/> is null.
    /// </returns>
    public static float? ToSingle(this string value, IFormatProvider provider)
    {
        if (value == null)
            return null;

        if (float.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, provider, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent 16-bit unsigned integer.
    /// </summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <returns>
    /// A 16-bit unsigned integer that is equivalent to the number in <paramref name="value"/>, or 0 (zero) if <paramref name="value"/> is null.
    /// </returns>
    public static ushort? ToUInt16(this string value)
    {
        if (value == null)
            return null;

        if (ushort.TryParse(value, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent 16-bit unsigned integer, using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>
    /// A 16-bit unsigned integer that is equivalent to the number in <paramref name="value"/>, or 0 (zero) if <paramref name="value"/> is null.
    /// </returns>
    public static ushort? ToUInt16(this string value, IFormatProvider provider)
    {
        if (value == null)
            return null;

        if (ushort.TryParse(value, NumberStyles.Integer, provider, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent 32-bit unsigned integer.
    /// </summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <returns>
    /// A 32-bit unsigned integer that is equivalent to the number in <paramref name="value"/>, or 0 (zero) if <paramref name="value"/> is null.
    /// </returns>
    public static uint? ToUInt32(this string value)
    {
        if (value == null)
            return null;

        if (uint.TryParse(value, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent 32-bit unsigned integer, using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>
    /// A 32-bit unsigned integer that is equivalent to the number in <paramref name="value"/>, or 0 (zero) if <paramref name="value"/> is null.
    /// </returns>
    public static uint? ToUInt32(this string value, IFormatProvider provider)
    {
        if (value == null)
            return null;

        if (uint.TryParse(value, NumberStyles.Integer, provider, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent 64-bit unsigned integer.
    /// </summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <returns>
    /// A 64-bit signed integer that is equivalent to the number in <paramref name="value"/>, or 0 (zero) if <paramref name="value"/> is null.
    /// </returns>
    public static ulong? ToUInt64(this string value)
    {
        if (value == null)
            return null;

        if (ulong.TryParse(value, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string representation of a number to an equivalent 64-bit unsigned integer, using the specified culture-specific formatting information.
    /// </summary>
    /// <param name="value">A string that contains the number to convert.</param>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <returns>
    /// A 64-bit unsigned integer that is equivalent to the number in <paramref name="value"/>, or 0 (zero) if <paramref name="value"/> is null.
    /// </returns>
    public static ulong? ToUInt64(this string value, IFormatProvider provider)
    {
        if (value == null)
            return null;

        if (ulong.TryParse(value, NumberStyles.Integer, provider, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string to an equivalent <see cref="TimeSpan"/> value.
    /// </summary>
    /// <param name="value">The string representation of a <see cref="TimeSpan"/>.</param>
    /// <returns>
    /// The <see cref="TimeSpan"/> equivalent of the <paramref name="value"/>, or <see cref="F:System.TimeSpan.Zero"/> if <paramref name="value"/> is null.
    /// </returns>
    public static TimeSpan? ToTimeSpan(this string value)
    {
        if (value == null)
            return null;

        if (TimeSpan.TryParse(value, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Converts the specified string to an equivalent <see cref="Guid"/> value.
    /// </summary>
    /// <param name="value">The string representation of a <see cref="Guid"/>.</param>
    /// <returns>
    /// The <see cref="Guid"/> equivalent of the <paramref name="value"/>, or <see cref="F:System.Guid.Empty"/> if <paramref name="value"/> is null.
    /// </returns>
    public static Guid? ToGuid(this string value)
    {
        if (value == null)
            return null;

        if (Guid.TryParse(value, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Try to convert the specified string to the specified type.
    /// </summary>
    /// <param name="input">The input string to convert.</param>
    /// <param name="value">The converted value.</param>
    /// <returns><c>true</c> if the value is converted; otherwise <c>false</c></returns>
    public static bool TryConvert<T>(this string input, out T value)
    {
        var type = typeof(T);

        var result = TryConvert(input, type, out var objectType);
        value = result ? (T)objectType : default(T);

        return result;
    }

    /// <summary>
    /// Try to convert the specified string to the specified type.
    /// </summary>
    /// <param name="input">The input string to convert.</param>
    /// <param name="type">The type to convert to.</param>
    /// <param name="value">The converted value.</param>
    /// <returns><c>true</c> if the value is converted; otherwise <c>false</c></returns>
    public static bool TryConvert(this string input, Type type, out object value)
    {
        // first try string
        if (type == typeof(string))
        {
            value = input;
            return true;
        }

        // check nullable
        if (input == null && type.IsNullable())
        {
            value = null;
            return true;
        }

        Type underlyingType = type.GetUnderlyingType();

        // convert by type
        if (underlyingType == typeof(bool))
        {
            value = input.ToBoolean();
            return value != null;
        }
        if (underlyingType == typeof(byte))
        {
            value = input.ToByte();
            return value != null;
        }
        if (underlyingType == typeof(DateTime))
        {
            value = input.ToDateTime();
            return value != null;
        }
        if (underlyingType == typeof(decimal))
        {
            value = input.ToDecimal();
            return value != null;
        }
        if (underlyingType == typeof(double))
        {
            value = input.ToDouble();
            return value != null;
        }
        if (underlyingType == typeof(short))
        {
            value = input.ToInt16();
            return value != null;
        }
        if (underlyingType == typeof(int))
        {
            value = input.ToInt32();
            return value != null;
        }
        if (underlyingType == typeof(long))
        {
            value = input.ToInt64();
            return value != null;
        }
        if (underlyingType == typeof(float))
        {
            value = input.ToSingle();
            return value != null;
        }
        if (underlyingType == typeof(ushort))
        {
            value = input.ToUInt16();
            return value != null;
        }
        if (underlyingType == typeof(uint))
        {
            value = input.ToUInt32();
            return value != null;
        }
        if (underlyingType == typeof(ulong))
        {
            value = input.ToUInt64();
            return value != null;
        }
        if (underlyingType == typeof(TimeSpan))
        {
            value = input.ToTimeSpan();
            return value != null;
        }
        if (underlyingType == typeof(Guid))
        {
            value = input.ToGuid();
            return value != null;
        }

        value = null;
        return false;
    }

}
