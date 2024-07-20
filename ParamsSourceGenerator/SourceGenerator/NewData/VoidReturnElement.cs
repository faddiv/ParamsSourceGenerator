using Foxy.Params.SourceGenerator.Rendering;

namespace Foxy.Params.SourceGenerator.NewData;

internal class VoidReturnElement : IReturnElement, IElement
{
    public static readonly VoidReturnElement Instance = new();
    private VoidReturnElement()
    {
        
    }

    public void ExecuteRenderer<TRenderOutput>(RendererBase<TRenderOutput> renderer, TRenderOutput output) where TRenderOutput : IRenderOutput
    {
        renderer.Render(this, output);
    }

    public override string ToString()
    {
        return "void";
    }
}
