using Foxy.Params.SourceGenerator.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Foxy.Params.SourceGenerator.NewData;

internal class ParameterElement(
    string name,
    ITypeElement type,
    ParameterModifier modifier,
    bool isNullable)
    : NamedElement(name), IEquatable<ParameterElement?>
{
    public ITypeElement Type { get; } = type;
    public ParameterModifier Modifier { get; } = modifier;
    public bool IsNullable { get; } = isNullable;

    public override void ExecuteRenderer<TRenderOutput>(RendererBase<TRenderOutput> renderer, TRenderOutput output)
    {
        renderer.Render(this, output);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ParameterElement);
    }

    public bool Equals(ParameterElement? other)
    {
        return other is not null &&
               base.Equals(other) &&
               EqualityComparer<ITypeElement>.Default.Equals(Type, other.Type) &&
               IsNullable == other.IsNullable &&
               Modifier == other.Modifier;
    }

    public override int GetHashCode()
    {
        int hashCode = 2062274795;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<ITypeElement>.Default.GetHashCode(Type);
        hashCode = hashCode * -1521134295 + Modifier.GetHashCode();
        hashCode = hashCode * -1521134295 + IsNullable.GetHashCode();
        return hashCode;
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append(Type);
        if(IsNullable)
        {
            builder.Append('?');
        }
        builder.Append(' ');
        builder.Append(Name);
        return builder.ToString();
    }
}
