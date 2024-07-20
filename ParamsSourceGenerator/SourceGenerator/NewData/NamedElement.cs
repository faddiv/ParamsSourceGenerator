using Foxy.Params.SourceGenerator.Rendering;
using System;
using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.NewData;

internal abstract class NamedElement(string name) : IElement, IEquatable<NamedElement?>
{
    public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));

    public override bool Equals(object? obj)
    {
        return Equals(obj as NamedElement);
    }

    public bool Equals(NamedElement? other)
    {
        return other is not null &&
               Name == other.Name;
    }

    public abstract void ExecuteRenderer<TRenderOutput>(RendererBase<TRenderOutput> renderer, TRenderOutput output) where TRenderOutput : IRenderOutput;

    public override int GetHashCode()
    {
        return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
    }

    public override string ToString()
    {
        return Name;
    }
}
