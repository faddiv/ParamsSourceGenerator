using Foxy.Params.SourceGenerator.Rendering;

namespace Foxy.Params.SourceGenerator.NewData;

internal class GlobalNamespaceElement : INamesapce
{
    public static GlobalNamespaceElement Instance = new();

    private GlobalNamespaceElement()
    {
    }

    public void ExecuteRenderer<TRenderOutput>(RendererBase<TRenderOutput> renderer, TRenderOutput output) where TRenderOutput : IRenderOutput
    {
        renderer.Render(this, output);
    }

    public override string ToString()
    {
        return "global::";
    }
}