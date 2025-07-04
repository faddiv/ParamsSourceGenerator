﻿using Foxy.Params.SourceGenerator.Helpers;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Foxy.Params.SourceGenerator.Data;

internal class MethodInfo : IEquatable<MethodInfo?>
{
    public required string ReturnType { get; init; }

    public string SpanArgumentType => ParamsArgument.GetFirstGenericType();

    public required ParameterInfo[] Parameters { get; init; }

    public ref ParameterInfo ParamsArgument => ref Parameters[^1];

    public required ReturnKind ReturnsKind { get; init; }

    public required GenericTypeInfo[] TypeConstraints { get; init; }

    public required string MethodName { get; init; }

    public required bool IsStatic { get; init; }

    public IEnumerable<ParameterInfo> GetFixedParameters()
    {
        return Parameters.Take(Parameters.Length - 1);
    }

    public IEnumerable<string> GetFixArguments()
    {
        return GetFixedParameters().Select(e => e.ToParameter()).ToList();
    }

    public string GetArgName()
    {
        return ParamsArgument.Name;
    }

    public string GetArgNameSpan()
    {
        return $"{ParamsArgument.Name}Span";
    }

    public string GetArgNameSpanInput()
    {
        return ParamsArgument.IsRefType
                    ? $"ref {GetArgNameSpan()}"
                    : GetArgNameSpan();
    }

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

    public static ParameterInfo[] GetArguments(IMethodSymbol methodSymbol)
    {
        return methodSymbol.Parameters
            .Select(arg => new ParameterInfo(
        type: TypeInfo.Create(arg.Type),
        name: arg.Name,
        refKind: arg.RefKind,
        isNullable: arg.NullableAnnotation == NullableAnnotation.Annotated))
            .ToArray();
    }

    public static GenericTypeInfo[] CreateTypeConstraints(ImmutableArray<ITypeSymbol> typeArguments)
    {
        var typeConstraintsList = new GenericTypeInfo[typeArguments.Length];
        for (int i = 0; i < typeArguments.Length; i++)
        {
            var typeArg = (ITypeParameterSymbol)typeArguments[i];
            typeConstraintsList[i] = new GenericTypeInfo
            {
                Type = typeArg.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                ConstraintType = SemanticHelpers.GetConstraintType(typeArg),
                ConstraintTypes = typeArg.ConstraintTypes.Select(e => e.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)).ToArray(),
                HasConstructorConstraint = typeArg.HasConstructorConstraint
            };
        }
        return typeConstraintsList;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as MethodInfo);
    }

    public bool Equals(MethodInfo? other)
    {
        return other is not null &&
               ReturnType == other.ReturnType &&
               SpanArgumentType == other.SpanArgumentType &&
               Parameters.SequenceEqual(other.Parameters) &&
               ReturnsKind == other.ReturnsKind &&
               TypeConstraints.SequenceEqual(other.TypeConstraints) &&
               MethodName == other.MethodName &&
               IsStatic == other.IsStatic;
    }

    public override int GetHashCode()
    {
        int hashCode = 1919567312;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ReturnType);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SpanArgumentType);
        hashCode = hashCode * -1521134295 + CollectionComparer.GetHashCode(Parameters);
        hashCode = hashCode * -1521134295 + ReturnsKind.GetHashCode();
        hashCode = hashCode * -1521134295 + CollectionComparer.GetHashCode(TypeConstraints);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MethodName);
        hashCode = hashCode * -1521134295 + IsStatic.GetHashCode();
        return hashCode;
    }
}
