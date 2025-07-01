using Foxy.Params.SourceGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foxy.Params.SourceGenerator.Data;

internal class GenericTypeInfo : IEquatable<GenericTypeInfo?>
{
    public required string Type { get; init; }

    public required ConstraintType ConstraintType { get; init; }
    public required string[] ConstraintTypes { get; init; }
    public required bool HasConstructorConstraint { get; init; }

    public bool HasConstraint()
    {
        return ConstraintType != ConstraintType.None
            || ConstraintTypes.Length > 0
            || HasConstructorConstraint;
    }

    public void WriteTo(SourceBuilder builder)
    {
        if (!HasConstraint())
        {
            return;
        }
        using var line = builder.StartLine();
        line.AddFormatted($"where {Type} : ");
        var commaSeparatedList = line.StartCommaSeparatedList();

        switch (ConstraintType)
        {
            case ConstraintType.Unmanaged:
                commaSeparatedList.AddElement("unmanaged");
                break;
            case ConstraintType.Struct:
                commaSeparatedList.AddElement("struct");
                break;
            case ConstraintType.Class:
                commaSeparatedList.AddElement("class");
                break;
            case ConstraintType.NotNull:
                commaSeparatedList.AddElement("notnull");
                break;
        }

        if (ConstraintTypes.Length > 0)
        {
            foreach (var item in ConstraintTypes)
            {
                commaSeparatedList.AddElement(item);
            }
        }
        if (HasConstructorConstraint)
        {
            commaSeparatedList.AddElement("new()");
        }
    }

    public override string ToString()
    {
        return Type;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as GenericTypeInfo);
    }

    public bool Equals(GenericTypeInfo? other)
    {
        return other is not null &&
               Type == other.Type &&
               ConstraintType == other.ConstraintType &&
               ConstraintTypes.SequenceEqual(other.ConstraintTypes) &&
               HasConstructorConstraint == other.HasConstructorConstraint;
    }

    public override int GetHashCode()
    {
        int hashCode = 1142259888;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Type);
        hashCode = hashCode * -1521134295 + ConstraintType.GetHashCode();
        hashCode = hashCode * -1521134295 + CollectionComparer.GetHashCode(ConstraintTypes);
        hashCode = hashCode * -1521134295 + HasConstructorConstraint.GetHashCode();
        return hashCode;
    }
}
