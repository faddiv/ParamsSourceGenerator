using Foxy.Params.SourceGenerator.SourceGenerator;
using System;

namespace Foxy.Params.SourceGenerator.Data;


internal class SuccessfulParamsCandidate : SuccessfulParams, IEquatable<SuccessfulParamsCandidate?>
{
    public required CandidateTypeInfo TypeInfo { get; init; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as SuccessfulParamsCandidate);
    }

    public bool Equals(SuccessfulParamsCandidate? other)
    {
        return other is not null &&
               TypeInfo.Equals(other.TypeInfo) &&
               base.Equals(other);
    }

    public override int GetHashCode()
    {
        int hashCode = 274651747;
        hashCode = hashCode * -1521134295 + TypeInfo.GetHashCode();
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        return hashCode;
    }
}

