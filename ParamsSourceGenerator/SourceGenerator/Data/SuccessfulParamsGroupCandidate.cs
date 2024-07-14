using Foxy.Params.SourceGenerator.Helpers;
using System;
using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.Data;

internal class SuccessfulParamsGroupCandidate : ParamsCandidate, IEquatable<SuccessfulParamsGroupCandidate?>
{
    public override bool HasErrors => false;

    public required CandidateTypeInfo TypeInfo { get; init; }
    
    public required IReadOnlyList<SuccessfulParams> ParamCanditates { get; init; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as SuccessfulParamsGroupCandidate);
    }

    public bool Equals(SuccessfulParamsGroupCandidate? other)
    {
        return other is not null &&
            TypeInfo.Equals(other.TypeInfo) &&
            CollectionComparer.GetFor(ParamCanditates).Equals(ParamCanditates, other.ParamCanditates);
    }

    public override int GetHashCode()
    {
        int hashCode = -1130635483;
        hashCode = hashCode * -1521134295 + TypeInfo.GetHashCode();
        hashCode = hashCode * -1521134295 + CollectionComparer.GetFor(ParamCanditates).GetHashCode(ParamCanditates);
        return hashCode;
    }
}

