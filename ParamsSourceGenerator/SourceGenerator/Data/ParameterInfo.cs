using Foxy.Params.SourceGenerator.Helpers;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.Data;

public class ParameterInfo : IEquatable<ParameterInfo?>
{
    public ParameterInfo(IParameterSymbol arg)
    {
        Type = arg.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        Name = arg.Name;
        RefKind = arg.RefKind;
        IsNullable = arg.NullableAnnotation == NullableAnnotation.Annotated;
    }

    public string Type { get; }
    public string Name { get; }
    public RefKind RefKind { get; }
    public bool IsNullable { get; }

    public string ToParameter()
    {
        return $"{SemanticHelpers.WithModifiers(Type, RefKind, IsNullable)} {Name}";
    }

    public string ToPassParameter()
    {
        return SemanticHelpers.WithModifiers(Name, GetPassParameterModifier(RefKind), false);
    }

    private static RefKind GetPassParameterModifier(RefKind refKind)
    {
        switch (refKind)
        {
            case RefKind.Ref:
                return RefKind.Ref;
            case RefKind.Out:
                return RefKind.Out;
            case RefKind.In:
            case RefKind.RefReadOnlyParameter:
                return RefKind.In;
            default:
                return RefKind.None;
        }
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ParameterInfo);
    }

    public bool Equals(ParameterInfo? other)
    {
        return other is not null &&
               Type == other.Type &&
               Name == other.Name &&
               RefKind == other.RefKind &&
               IsNullable == other.IsNullable;
    }

    public override int GetHashCode()
    {
        int hashCode = -1345310773;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Type);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
        hashCode = hashCode * -1521134295 + RefKind.GetHashCode();
        hashCode = hashCode * -1521134295 + IsNullable.GetHashCode();
        return hashCode;
    }
}

