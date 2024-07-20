using Foxy.Params.SourceGenerator.Helpers;
using Foxy.Params.SourceGenerator.NewData;
using Foxy.Params.SourceGenerator.Rendering;
using System;
using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.Data;

internal class SuccessfulParamsGroupCandidateV2 : ParamsCandidate, IEquatable<SuccessfulParamsGroupCandidateV2?>, IElement
{
    public override bool HasErrors => false;

    public required ClassTypeElement TypeInfo { get; init; }

    public required IReadOnlyList<SuccessfulParamsV2> ParamCanditates { get; init; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as SuccessfulParamsGroupCandidateV2);
    }

    public bool Equals(SuccessfulParamsGroupCandidateV2? other)
    {
        return other is not null &&
            TypeInfo.Equals(other.TypeInfo) &&
            CollectionComparer.Equals(ParamCanditates, other.ParamCanditates);
    }

    public void ExecuteRenderer<TRenderOutput>(RendererBase<TRenderOutput> renderer, TRenderOutput output) where TRenderOutput : IRenderOutput
    {
        renderer.Render(this, output);
    }

    public override int GetHashCode()
    {
        int hashCode = -1130635483;
        hashCode = hashCode * -1521134295 + TypeInfo.GetHashCode();
        hashCode = hashCode * -1521134295 + CollectionComparer.GetHashCode(ParamCanditates);
        return hashCode;
    }
}