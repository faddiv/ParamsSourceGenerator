using Foxy.Params.SourceGenerator.NewData;
using Foxy.Params.SourceGenerator.Rendering;
using System;

namespace Foxy.Params.SourceGenerator.Data;

internal class SuccessfulParamsV2 : ParamsCandidate, IElement, IEquatable<SuccessfulParamsV2?>
{
    public override bool HasErrors => false;

    public required MethodElement MethodInfo { get; init; }

    public required int MaxOverrides { get; init; }

    public required bool HasParams { get; init; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as SuccessfulParamsV2);
    }

    public bool Equals(SuccessfulParamsV2? other)
    {
        return other is not null &&
               MaxOverrides == other.MaxOverrides &&
               HasParams == other.HasParams &&
               MethodInfo.Equals(other.MethodInfo);
    }

    public void ExecuteRenderer<TRenderOutput>(RendererBase<TRenderOutput> renderer, TRenderOutput output) where TRenderOutput : IRenderOutput
    {
        renderer.Render(this, output);
    }

    public override int GetHashCode()
    {
        int hashCode = 274651747;
        hashCode = hashCode * -1521134295 + MaxOverrides.GetHashCode();
        hashCode = hashCode * -1521134295 + HasParams.GetHashCode();
        hashCode = hashCode * -1521134295 + MethodInfo.GetHashCode();
        return hashCode;
    }
}

