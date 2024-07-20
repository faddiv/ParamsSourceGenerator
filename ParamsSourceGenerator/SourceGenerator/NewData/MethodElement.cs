using Foxy.Params.SourceGenerator.Helpers;
using Foxy.Params.SourceGenerator.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foxy.Params.SourceGenerator.NewData;

internal class MethodElement(
    string name,
    ClassTypeElement parent,
    IGenericElement[]? genericArguments,
    IReturnElement @return,
    ParameterElement[]? parameters)
    : NamedElement(name), IHierarchicalElement<ClassTypeElement>, IEquatable<MethodElement?>
{
    public ClassTypeElement Parent { get; } = parent;

    public IReturnElement Return { get; } = @return;

    public IGenericElement[] GenericArguments { get; } = genericArguments ?? [];

    public ParameterElement[] Parameters { get; } = parameters ?? [];

    public override bool Equals(object? obj)
    {
        return Equals(obj as MethodElement);
    }

    public bool Equals(MethodElement? other)
    {
        return other is not null &&
               base.Equals(other) &&
               EqualityComparer<ClassTypeElement>.Default.Equals(Parent, other.Parent) &&
               EqualityComparer<IReturnElement>.Default.Equals(Return, other.Return) &&
               CollectionComparer.Equals(GenericArguments, other.GenericArguments) &&
               CollectionComparer.Equals(Parameters, other.Parameters);
    }

    public override void ExecuteRenderer<TRenderOutput>(RendererBase<TRenderOutput> renderer, TRenderOutput output)
    {
        renderer.Render(this, output);
    }

    public override int GetHashCode()
    {
        int hashCode = 1500428901;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<ClassTypeElement>.Default.GetHashCode(Parent);
        hashCode = hashCode * -1521134295 + EqualityComparer<IReturnElement>.Default.GetHashCode(Return);
        hashCode = hashCode * -1521134295 + CollectionComparer.GetHashCode(GenericArguments);
        hashCode = hashCode * -1521134295 + CollectionComparer.GetHashCode(Parameters);
        return hashCode;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append(Return);
        builder.Append(' ');
        builder.Append(Name);
        if (GenericArguments.Length > 0)
        {
            builder.Append('<');
            builder.Append(string.Join(", ", GenericArguments.Select(e => e.ToString())));
            builder.Append('>');
        }
        builder.Append('(');
        builder.Append(string.Join(", ", Parameters.Select(e => e.ToString())));
        builder.Append(')');
        return builder.ToString();
    }
}
