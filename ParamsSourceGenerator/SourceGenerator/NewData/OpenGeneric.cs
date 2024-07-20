using Foxy.Params.SourceGenerator.Helpers;
using Foxy.Params.SourceGenerator.Rendering;
using System;
using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.NewData;

internal class OpenGeneric(
    string name,
    ITypeElement[]? typeConstraints,
    bool hasValueTypeConstraint = false,
    bool hasUnmanagedTypeConstraint = false,
    bool hasNotNullConstraint = false,
    bool hasConstructorConstraint = false)
    : NamedElement(name), IGenericElement, IEquatable<OpenGeneric?>
{
    /// <summary>
    /// True if the value type constraint (<c>struct</c>) was specified for the type parameter.
    /// </summary>
    public bool HasValueTypeConstraint { get; } = hasValueTypeConstraint;

    /// <summary>
    /// True if the value type constraint (<c>unmanaged</c>) was specified for the type parameter.
    /// </summary>
    public bool HasUnmanagedTypeConstraint { get; } = hasUnmanagedTypeConstraint;

    /// <summary>
    /// True if the notnull constraint (<c>notnull</c>) was specified for the type parameter.
    /// </summary>
    public bool HasNotNullConstraint { get; } = hasNotNullConstraint;

    /// <summary>
    /// True if the parameterless constructor constraint (<c>new()</c>) was specified for the type parameter.
    /// </summary>
    public bool HasConstructorConstraint { get; } = hasConstructorConstraint;

    public ITypeElement[] TypeConstraints { get; } = typeConstraints ?? [];

    public override bool Equals(object? obj)
    {
        return Equals(obj as OpenGeneric);
    }

    public override void ExecuteRenderer<TRenderOutput>(RendererBase<TRenderOutput> renderer, TRenderOutput output)
    {
        renderer.Render(this, output);
    }

    public bool Equals(OpenGeneric? other)
    {
        return other is not null &&
               base.Equals(other) &&
               EqualityComparer<bool>.Default.Equals(HasNotNullConstraint, other.HasNotNullConstraint) &&
               EqualityComparer<bool>.Default.Equals(HasConstructorConstraint, other.HasConstructorConstraint) &&
               EqualityComparer<bool>.Default.Equals(HasUnmanagedTypeConstraint, other.HasUnmanagedTypeConstraint) &&
               EqualityComparer<bool>.Default.Equals(HasValueTypeConstraint, other.HasValueTypeConstraint) &&
               CollectionComparer.Equals(TypeConstraints, other.TypeConstraints);
    }

    public override int GetHashCode()
    {
        int hashCode = -1255882849;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
        hashCode = hashCode * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(HasNotNullConstraint);
        hashCode = hashCode * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(HasConstructorConstraint);
        hashCode = hashCode * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(HasUnmanagedTypeConstraint);
        hashCode = hashCode * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(HasValueTypeConstraint);
        hashCode = hashCode * -1521134295 + CollectionComparer.GetHashCode(TypeConstraints);
        return hashCode;
    }
}
