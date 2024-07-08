namespace System.Diagnostics.CodeAnalysis;

/// <summary>
/// Specifies that when a method returns System.Diagnostics.CodeAnalysis.NotNullWhenAttribute.ReturnValue, the parameter will not be null even if the corresponding type allows it.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
internal sealed class NotNullWhenAttribute : Attribute
{
    //
    // Summary:
    //     
    //
    // Parameters:
    //   returnValue:
    //     
    /// <summary>
    /// Initializes the attribute with the specified return value condition.
    /// </summary>
    /// <param name="returnValue">
    /// The return value condition. If the method returns this value, the associated parameter will not be null.
    /// </param>
    public NotNullWhenAttribute(bool returnValue)
        => ReturnValue = returnValue;

    /// <summary>
    /// The return value condition. If the method returns this value, the associated parameter will not be null.
    /// </summary>
    public bool ReturnValue { get; }
}
