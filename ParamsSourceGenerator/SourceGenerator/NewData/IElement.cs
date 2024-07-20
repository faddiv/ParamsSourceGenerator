using Foxy.Params.SourceGenerator.Rendering;

namespace Foxy.Params.SourceGenerator.NewData;

internal interface IElement
{
    void ExecuteRenderer<TRenderOutput>(RendererBase<TRenderOutput> renderer, TRenderOutput output)
        where TRenderOutput : IRenderOutput;
}


