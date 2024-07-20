using Foxy.Params.SourceGenerator.Rendering;
using System;
using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.NewData;

internal class ClosedGeneric(ITypeElement typeInfo) : IGenericElement, IEquatable<ClosedGeneric?>
{
    public ITypeElement TypeInfo { get; } = typeInfo;

    public void ExecuteRenderer<TRenderOutput>(RendererBase<TRenderOutput> renderer, TRenderOutput output) where TRenderOutput : IRenderOutput
    {
        renderer.Render(this, output);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ClosedGeneric);
    }

    public bool Equals(ClosedGeneric? other)
    {
        return other is not null &&
               EqualityComparer<ITypeElement>.Default.Equals(TypeInfo, other.TypeInfo);
    }

    public override int GetHashCode()
    {
        return -1399474619 + EqualityComparer<ITypeElement>.Default.GetHashCode(TypeInfo);
    }

    public override string ToString()
    {
        return TypeInfo.ToString();
    }
}


