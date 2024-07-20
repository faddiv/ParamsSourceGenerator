using Foxy.Params.SourceGenerator.Data;
using Foxy.Params.SourceGenerator.NewData;

namespace Foxy.Params.SourceGenerator.Rendering;

internal class RendererBase<TRenderOutput>
    where TRenderOutput : IRenderOutput
{
    public void DefaultRender(IElement _, TRenderOutput __)
    {

    }

    public virtual void Render(SuccessfulParamsGroupCandidateV2 element, TRenderOutput output)
    {
        DefaultRender(element, output);
    }

    public virtual void Render(SuccessfulParamsV2 element, TRenderOutput output)
    {
        DefaultRender(element, output);
    }

    public virtual void Render(NamespaceElement element, TRenderOutput output)
    {
        DefaultRender(element, output);
    }

    public void Render(KeywordTypeConstraint element, TRenderOutput output)
    {
        DefaultRender(element, output);
    }

    public virtual void Render(MethodElement element, TRenderOutput output)
    {
        DefaultRender(element, output);
    }

    public virtual void Render(ClassTypeElement element, TRenderOutput output)
    {
        DefaultRender(element, output);
    }

    public virtual void Render(OpenGeneric element, TRenderOutput output)
    {
        DefaultRender(element, output);
    }

    public virtual void Render(ParameterElement element, TRenderOutput output)
    {
        DefaultRender(element, output);
    }

    public virtual void Render(ClosedGeneric element, TRenderOutput output)
    {
        DefaultRender(element, output);
    }

    public virtual void Render(GlobalNamespaceElement element, TRenderOutput output)
    {
        DefaultRender(element, output);
    }

    public virtual void Render(KeywordTypeElement element, TRenderOutput output)
    {
        DefaultRender(element, output);
    }

    public virtual void Render(TypedReturnElement element, TRenderOutput output)
    {
        DefaultRender(element, output);
    }

    public virtual void Render(TypedTypeConstraint element, TRenderOutput output)
    {
        DefaultRender(element, output);
    }

    public virtual void Render(VoidReturnElement element, TRenderOutput output)
    {
        DefaultRender(element, output);
    }
}
