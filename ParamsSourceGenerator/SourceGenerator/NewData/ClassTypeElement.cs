using Foxy.Params.SourceGenerator.Helpers;
using Foxy.Params.SourceGenerator.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foxy.Params.SourceGenerator.NewData;

internal class ClassTypeElement(
    string name,
    ITypeOrNamesapce parent,
    IGenericElement[]? genericArguments) 
    : NamedElement(name), ITypeElement, ITypeOrMemeber, ITypeOrNamesapce, IEquatable<ClassTypeElement?>
{
    public ITypeOrNamesapce Parent { get; } = parent;

    public IGenericElement[] GenericArguments { get; } = genericArguments ?? [];

    public override void ExecuteRenderer<TRenderOutput>(RendererBase<TRenderOutput> renderer, TRenderOutput output)
    {
        renderer.Render(this, output);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ClassTypeElement);
    }

    public bool Equals(ClassTypeElement? other)
    {
        return other is not null &&
               base.Equals(other) &&
               EqualityComparer<ITypeOrNamesapce?>.Default.Equals(Parent, other.Parent) &&
               CollectionComparer.Equals(GenericArguments, other.GenericArguments);
    }

    public override int GetHashCode()
    {
        int hashCode = -251374819;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<ITypeOrNamesapce?>.Default.GetHashCode(Parent);
        hashCode = hashCode * -1521134295 + CollectionComparer.GetHashCode(GenericArguments);
        return hashCode;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        if(Parent is not GlobalNamespaceElement)
        {
            builder.Append(Parent.ToString());
        }

        builder.Append(Name);
        if(GenericArguments.Length > 0)
        {
            builder.Append('<');
            builder.Append(string.Join(", ", GenericArguments.Select(e => e.ToString())));
            builder.Append('>');
        }
        return builder.ToString();
    }
}
