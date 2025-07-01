using Foxy.Params.SourceGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foxy.Params.SourceGenerator.Data;

internal class SuccessfulParamsGroupCandidate : ParamsCandidate, IEquatable<SuccessfulParamsGroupCandidate?>
{
    public override bool HasErrors => false;

    public required CandidateTypeInfo TypeInfo { get; init; }
    
    public required SuccessfulParams[] ParamCandidates { get; init; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as SuccessfulParamsGroupCandidate);
    }

    public bool Equals(SuccessfulParamsGroupCandidate? other)
    {
        return other is not null &&
            TypeInfo.Equals(other.TypeInfo) &&
            ParamCandidates.SequenceEqual(other.ParamCandidates);
    }

    public override int GetHashCode()
    {
        int hashCode = -1130635483;
        hashCode = hashCode * -1521134295 + TypeInfo.GetHashCode();
        hashCode = hashCode * -1521134295 + CollectionComparer.GetHashCode(ParamCandidates);
        return hashCode;
    }
}

