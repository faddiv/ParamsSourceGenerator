using Foxy.Params.SourceGenerator.Helpers;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.Data;

public class ParameterInfo(string type, string name, RefKind refKind, bool isNullable) : IEquatable<ParameterInfo?>
{
    public string Type { get; } = type;

    public string Name { get; } = name;

    public RefKind RefKind { get; } = refKind;

    public bool IsNullable { get; } = isNullable;

    public bool IsSpanRefType => RefKind is RefKind.Ref or RefKind.RefReadOnlyParameter;

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
