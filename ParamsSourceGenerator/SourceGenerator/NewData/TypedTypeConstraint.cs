using Foxy.Params.SourceGenerator.Rendering;
using System;
using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.NewData;

internal class TypedTypeConstraint(ClassTypeElement type) : ITypeConstraint, IElement, IEquatable<TypedTypeConstraint?>
{
    public ClassTypeElement Type { get; } = type;

    public void ExecuteRenderer<TRenderOutput>(RendererBase<TRenderOutput> renderer, TRenderOutput output) where TRenderOutput : IRenderOutput
    {
        renderer.Render(this, output);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as TypedTypeConstraint);
    }

    public bool Equals(TypedTypeConstraint? other)
    {
        return other is not null &&
               EqualityComparer<ClassTypeElement>.Default.Equals(Type, other.Type);
    }

    public override int GetHashCode()
    {
        return 2049151605 + EqualityComparer<ClassTypeElement>.Default.GetHashCode(Type);
    }

    public override string ToString()
    {
        return Type.ToString();
    }
}