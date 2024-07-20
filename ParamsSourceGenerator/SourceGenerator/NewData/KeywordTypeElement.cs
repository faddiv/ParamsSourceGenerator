using Foxy.Params.SourceGenerator.Rendering;
using System;
using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.NewData;

internal class KeywordTypeElement(string keyword) : IElement, ITypeElement, IEquatable<KeywordTypeElement?>
{
    public string Keyword { get; } = keyword;

    public void ExecuteRenderer<TRenderOutput>(RendererBase<TRenderOutput> renderer, TRenderOutput output) where TRenderOutput : IRenderOutput
    {
        renderer.Render(this, output);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as KeywordTypeElement);
    }

    public bool Equals(KeywordTypeElement? other)
    {
        return other is not null &&
               Keyword == other.Keyword;
    }

    public override int GetHashCode()
    {
        return -303665852 + EqualityComparer<string>.Default.GetHashCode(Keyword);
    }

    public override string ToString()
    {
        return Keyword;
    }
}
