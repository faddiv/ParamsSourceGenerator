using Foxy.Params.SourceGenerator.Helpers;
using System;
using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.Data;

public class TypeConstrainInfo : IEquatable<TypeConstrainInfo?>
{
    public string Type { get; set; }
    public List<string> Constraints { get; set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as TypeConstrainInfo);
    }

    public bool Equals(TypeConstrainInfo? other)
    {
        return other is not null &&
               Type == other.Type &&
               CollectionComparer.GetFor(Constraints).Equals(Constraints, other.Constraints);
    }

    public override int GetHashCode()
    {
        int hashCode = 1814578080;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Type);
        hashCode = hashCode * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(Constraints);
        return hashCode;
    }

    public static bool operator ==(TypeConstrainInfo? left, TypeConstrainInfo? right)
    {
        return EqualityComparer<TypeConstrainInfo>.Default.Equals(left, right);
    }

    public static bool operator !=(TypeConstrainInfo? left, TypeConstrainInfo? right)
    {
        return !(left == right);
    }
}

