using System;

namespace Foxy.Params;

/// <summary>
/// Attribute applied to a method to indicate that overrides should be generated.
/// </summary>
/// <remarks>
/// The method must have ReadOnlySpan{T} as the last parameter.
/// </remarks>
[AttributeUsage(validOn: AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class ParamsAttribute : Attribute
{
    /// <summary>
    /// Defines how many overrides are generated. The default is 3.
    /// </summary>
    public int MaxOverrides { get; set; } = 3;

    /// <summary>
    /// Indicates whether the default params override is generated. The default is true.
    /// </summary>
    public bool HasParams { get; set; } = true;
}