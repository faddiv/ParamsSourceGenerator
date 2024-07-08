using System.ComponentModel;

namespace System.Runtime.CompilerServices;

/// <summary>
/// Specifies that a type has required members or that a member is required.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class RequiredMemberAttribute : Attribute
{
    /// <summary>
    /// Initializes a System.Runtime.CompilerServices.RequiredMemberAttribute instance with default values.
    /// </summary>
    public RequiredMemberAttribute() { }
}

