using Foxy.Params.SourceGenerator.Helpers;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Foxy.Params.SourceGenerator.Data;

internal class DerivedData : IEquatable<DerivedData?>
{
    public required string ReturnType { get; init; }

    public required string SpanArgumentType { get; init; }

    public required List<ParameterInfo> ParameterInfos { get; init; }

    public required List<string> FixArguments { get; init; }

    public required ReturnKind ReturnsKind { get; init; }

    public required List<string> TypeArguments { get; init; }

    public required List<TypeConstrainInfo> TypeConstraints { get; init; }

    public required string ArgName { get; init; }

    public required string ArgNameSpan { get; init; }

    public required string ArgNameSpanInput { get; init; }

    public required string MethodName { get; init; }

    public required bool IsStatic { get; init; }

    public static string CreateReturnTypeFor(IMethodSymbol methodSymbol)
    {
        var returnType = methodSymbol.ReturnType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var isNullable = methodSymbol.ReturnType.NullableAnnotation == NullableAnnotation.Annotated;
        if (methodSymbol.ReturnsByRef)
        {
            return SemanticHelpers.WithModifiers(returnType, RefKind.Ref, isNullable);
        }
        else if (methodSymbol.ReturnsByRefReadonly)
        {
            return SemanticHelpers.WithModifiers(returnType, RefKind.RefReadOnlyParameter, isNullable);
        }

        return SemanticHelpers.WithModifiers(returnType, RefKind.None, isNullable);
    }

    public static string GetSpanArgumentType(IParameterSymbol spanParam)
    {
        var spanType = spanParam.Type as INamedTypeSymbol;
        SemanticHelpers.AssertNotNull(spanType);
        var spanTypeArgument = spanType.TypeArguments.First();
        string spanTypeName = spanTypeArgument.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        bool isNullable = spanTypeArgument.NullableAnnotation == NullableAnnotation.Annotated;
        return SemanticHelpers.WithModifiers(spanTypeName, RefKind.None, isNullable);
    }

    public static List<ParameterInfo> GetNonParamsArguments(IMethodSymbol methodSymbol)
    {
        return methodSymbol.Parameters
            .Take(methodSymbol.Parameters.Length - 1)
            .Select(arg => new ParameterInfo(arg))
            .ToList();
    }

    public static List<TypeConstrainInfo> CreateTypeConstraints(ImmutableArray<ITypeSymbol> typeArguments)
    {
        var typeConstraintsList = new List<TypeConstrainInfo>();
        foreach (var typeArg in typeArguments.Cast<ITypeParameterSymbol>())
        {
            var typeConstraints = new List<string>();
            if (typeArg.HasUnmanagedTypeConstraint)
            {
                typeConstraints.Add("unmanaged");
            }
            else if (typeArg.HasValueTypeConstraint)
            {
                typeConstraints.Add("struct");
            }
            else if (typeArg.HasReferenceTypeConstraint)
            {
                typeConstraints.Add("class");
            }
            else if (typeArg.HasNotNullConstraint)
            {
                typeConstraints.Add("notnull");
            }
            if (typeArg.ConstraintTypes.Length > 0)
            {
                foreach (var item in typeArg.ConstraintTypes)
                {
                    typeConstraints.Add(item.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                }
            }
            if (typeArg.HasConstructorConstraint)
            {
                typeConstraints.Add("new()");
            }
            if (typeConstraints.Count > 0)
            {
                typeConstraintsList.Add(new TypeConstrainInfo
                {
                    Type = typeArg.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    Constraints = typeConstraints
                });
            }
        }
        return typeConstraintsList;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as DerivedData);
    }

    public bool Equals(DerivedData? other)
    {
        return other is not null &&
               ReturnType == other.ReturnType &&
               SpanArgumentType == other.SpanArgumentType &&
               CollectionComparer.GetFor(ParameterInfos).Equals(ParameterInfos, other.ParameterInfos) &&
               CollectionComparer.GetFor(FixArguments).Equals(FixArguments, other.FixArguments) &&
               ReturnsKind == other.ReturnsKind &&
               CollectionComparer.GetFor(TypeArguments).Equals(TypeArguments, other.TypeArguments) &&
               CollectionComparer.GetFor(TypeConstraints).Equals(TypeConstraints, other.TypeConstraints) &&
               ArgName == other.ArgName &&
               ArgNameSpan == other.ArgNameSpan &&
               ArgNameSpanInput == other.ArgNameSpanInput &&
               MethodName == other.MethodName &&
               IsStatic == other.IsStatic;
    }

    public override int GetHashCode()
    {
        int hashCode = 1919567312;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ReturnType);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SpanArgumentType);
        hashCode = hashCode * -1521134295 + CollectionComparer.GetFor(ParameterInfos).GetHashCode(ParameterInfos);
        hashCode = hashCode * -1521134295 + CollectionComparer.GetFor(FixArguments).GetHashCode(FixArguments);
        hashCode = hashCode * -1521134295 + ReturnsKind.GetHashCode();
        hashCode = hashCode * -1521134295 + CollectionComparer.GetFor(TypeArguments).GetHashCode(TypeArguments);
        hashCode = hashCode * -1521134295 + CollectionComparer.GetFor(TypeConstraints).GetHashCode(TypeConstraints);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ArgName);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ArgNameSpan);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ArgNameSpanInput);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MethodName);
        hashCode = hashCode * -1521134295 + IsStatic.GetHashCode();
        return hashCode;
    }
}
