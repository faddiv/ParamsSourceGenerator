using Foxy.Params.SourceGenerator.Helpers;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Foxy.Params.SourceGenerator.Data;

internal class MethodInfo : IEquatable<MethodInfo?>
{
    public required string ReturnType { get; init; }

    public required string SpanArgumentType { get; init; }

    public required ParameterInfo[] Parameters { get; init; }

    public ref ParameterInfo ParamsArgument => ref Parameters[^1];

    public required ReturnKind ReturnsKind { get; init; }

    public required List<string> TypeArguments { get; init; }

    public required List<TypeConstrainInfo> TypeConstraints { get; init; }

    public required string MethodName { get; init; }

    public required bool IsStatic { get; init; }

    public ReadOnlySpan<ParameterInfo> GetFixedParameters()
    {
        return new(Parameters, 0, Parameters.Length - 1);
    }

    public List<string> GetFixArguments()
    {
        return GetFixedParameters().ToImmutableArray().Select(e => e.ToParameter()).ToList();
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
        type: arg.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
        name: arg.Name,
        refKind: arg.RefKind,
        isNullable: arg.NullableAnnotation == NullableAnnotation.Annotated))
            .ToArray();
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
        return Equals(obj as MethodInfo);
    }

    public bool Equals(MethodInfo? other)
    {
        return other is not null &&
               ReturnType == other.ReturnType &&
               SpanArgumentType == other.SpanArgumentType &&
               CollectionComparer.GetFor(Parameters).Equals(Parameters, other.Parameters) &&
               ReturnsKind == other.ReturnsKind &&
               CollectionComparer.GetFor(TypeArguments).Equals(TypeArguments, other.TypeArguments) &&
               CollectionComparer.GetFor(TypeConstraints).Equals(TypeConstraints, other.TypeConstraints) &&
               MethodName == other.MethodName &&
               IsStatic == other.IsStatic;
    }

    public override int GetHashCode()
    {
        int hashCode = 1919567312;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ReturnType);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SpanArgumentType);
        hashCode = hashCode * -1521134295 + CollectionComparer.GetFor(Parameters).GetHashCode(Parameters);
        hashCode = hashCode * -1521134295 + CollectionComparer.GetFor(GetFixArguments()).GetHashCode(GetFixArguments());
        hashCode = hashCode * -1521134295 + ReturnsKind.GetHashCode();
        hashCode = hashCode * -1521134295 + CollectionComparer.GetFor(TypeArguments).GetHashCode(TypeArguments);
        hashCode = hashCode * -1521134295 + CollectionComparer.GetFor(TypeConstraints).GetHashCode(TypeConstraints);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MethodName);
        hashCode = hashCode * -1521134295 + IsStatic.GetHashCode();
        return hashCode;
    }
}
