using System;

namespace Foxy.Params.SourceGenerator.Data;

internal class SuccessfulParams : ParamsCandidate, IEquatable<SuccessfulParams?>
{
    public override bool HasErrors => false;

    public required DerivedData DerivedData { get; init; }

    public required int MaxOverrides { get; init; }

    public required bool HasParams { get; init; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as SuccessfulParams);
    }

    public bool Equals(SuccessfulParams? other)
    {
        return other is not null &&
               MaxOverrides == other.MaxOverrides &&
               HasParams == other.HasParams &&
               DerivedData.Equals(other.DerivedData);
    }

    public override int GetHashCode()
    {
        int hashCode = 274651747;
        hashCode = hashCode * -1521134295 + MaxOverrides.GetHashCode();
        hashCode = hashCode * -1521134295 + HasParams.GetHashCode();
        hashCode = hashCode * -1521134295 + DerivedData.GetHashCode();
        return hashCode;
    }
}

