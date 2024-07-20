using Foxy.Params.SourceGenerator.Rendering;
using System;
using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.NewData;

internal class TypedReturnElement(ITypeElement type, ReturnKindEnum returnKind = ReturnKindEnum.Normal)
    : IReturnElement, IElement, IEquatable<TypedReturnElement?>
{
    public ITypeElement Type { get; } = type;
    
    public ReturnKindEnum ReturnKind { get; } = returnKind;

    public void ExecuteRenderer<TRenderOutput>(RendererBase<TRenderOutput> renderer, TRenderOutput output) where TRenderOutput : IRenderOutput
    {
        renderer.Render(this, output);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as TypedReturnElement);
    }

    public bool Equals(TypedReturnElement? other)
    {
        return other is not null &&
               EqualityComparer<ITypeElement>.Default.Equals(Type, other.Type) &&
               ReturnKind == other.ReturnKind;
    }

    public override int GetHashCode()
    {
        int hashCode = 945624170;
        hashCode = hashCode * -1521134295 + EqualityComparer<ITypeElement>.Default.GetHashCode(Type);
        hashCode = hashCode * -1521134295 + ReturnKind.GetHashCode();
        return hashCode;
    }

    public override string ToString()
    {
        return ReturnKind switch
        {
            ReturnKindEnum.Normal => Type.ToString(),
            ReturnKindEnum.Ref => $"ref {Type}",
            _ => throw new NotImplementedException(),
        };
    }
}
