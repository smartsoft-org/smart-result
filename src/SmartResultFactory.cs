namespace SmartResult;

/// <summary>
/// Provides factory methods for creating instances of <see cref="SmartResult{TValue}"/>.
/// </summary>
public static class SmartResultFactory
{
     /// <summary>
    /// Creates a new instance of <see cref="SmartResult{TValue}"/> with a value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value">The value to wrap.</param>
    /// <returns>An instance of <see cref="SmartResult{TValue}"/> containing the provided value.</returns>
    public static SmartResult<TValue> From<TValue>(TValue value)
    {
        return value;
    }
}
