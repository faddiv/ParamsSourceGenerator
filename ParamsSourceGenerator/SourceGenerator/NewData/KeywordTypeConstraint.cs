using Foxy.Params.SourceGenerator.Rendering;

namespace Foxy.Params.SourceGenerator.NewData;

internal class KeywordTypeConstraint(string name) : NamedElement(name), ITypeConstraint
{
    public override void ExecuteRenderer<TRenderOutput>(RendererBase<TRenderOutput> renderer, TRenderOutput output)
    {
        renderer.Render(this, output);
    }
}
