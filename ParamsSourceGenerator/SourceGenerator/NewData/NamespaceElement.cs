using Foxy.Params.SourceGenerator.Rendering;
using System;
using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.NewData;

internal class NamespaceElement(string name, INamesapce parent)
    : NamedElement(name), INamesapce, ITypeOrNamesapce, IEquatable<NamespaceElement?>
{
    public INamesapce Parent { get; } = parent;

    public override bool Equals(object? obj)
    {
        return Equals(obj as NamespaceElement);
    }

    public bool Equals(NamespaceElement? other)
    {
        return other is not null &&
               base.Equals(other) &&
               EqualityComparer<INamesapce>.Default.Equals(Parent, other.Parent);
    }

    public override void ExecuteRenderer<TRenderOutput>(RendererBase<TRenderOutput> renderer, TRenderOutput output)
    {
        renderer.Render(this, output);
    }

    public override int GetHashCode()
    {
        int hashCode = -427245389;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<INamesapce>.Default.GetHashCode(Parent);
        return hashCode;
    }

    public override string ToString()
    {
        if(Parent is GlobalNamespaceElement)
        {
            return $"{Parent}{Name}";
        }
        return $"{Parent}.{Name}";
    }
}
