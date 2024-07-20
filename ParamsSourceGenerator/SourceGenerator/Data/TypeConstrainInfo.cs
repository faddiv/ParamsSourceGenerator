using Foxy.Params.SourceGenerator.Helpers;
using System;
using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.Data;

public class TypeConstrainInfo : IEquatable<TypeConstrainInfo?>
{
    public required string Type { get; init; }

    public required List<string> Constraints { get; init; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as TypeConstrainInfo);
    }

    public bool Equals(TypeConstrainInfo? other)
    {
        return other is not null &&
               Type == other.Type &&
               CollectionComparer.Equals(Constraints, other.Constraints);
    }

    public override int GetHashCode()
    {
        int hashCode = 1814578080;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Type);
        hashCode = hashCode * -1521134295 + CollectionComparer.GetHashCode(Constraints);
        return hashCode;
    }
}

