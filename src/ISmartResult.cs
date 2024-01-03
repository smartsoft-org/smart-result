namespace SmartResult;

public interface ISmartResult<out TValue> : ISmartResult
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    TValue Value { get; }
}

/// <summary>
/// Type-less interface for the <see cref="SmartResult"/> object.
/// </summary>
/// <remarks>
/// This interface is intended for use when the underlying type of the <see cref="SmartResult"/> object is unknown.
/// </remarks>
public interface ISmartResult
{
    /// <summary>
    /// Gets the list of errors.
    /// </summary>
    List<Error>? Errors { get; }

    /// <summary>
    /// Gets a value indicating whether the state is error.
    /// </summary>
    bool IsError { get; }
}
