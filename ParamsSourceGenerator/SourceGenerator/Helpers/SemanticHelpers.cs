using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Foxy.Params.SourceGenerator.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Foxy.Params.SourceGenerator.Helpers;

internal static class SemanticHelpers
{
    public static void AssertNotNull([NotNull] object? typeInfo)
    {
        if (typeInfo is null)
            throw new ArgumentNullException(nameof(typeInfo));
    }

    public static bool TryGetAttribute(
        MemberDeclarationSyntax candidate,
        string attributeName,
        SemanticModel semanticModel,
        CancellationToken cancellationToken,
        [NotNullWhen(true)]out AttributeSyntax? value)
    {
        foreach (AttributeListSyntax attributeList in candidate.AttributeLists)
        {
            foreach (AttributeSyntax attribute in attributeList.Attributes)
            {
                SymbolInfo info = semanticModel.GetSymbolInfo(attribute, cancellationToken);
                ISymbol? symbol = info.Symbol;

                if (symbol is IMethodSymbol method
                    && method.ContainingType.ToDisplayString().Equals(attributeName, StringComparison.Ordinal))
                {
                    value = attribute;
                    return true;
                }
            }
        }

        value = null;
        return false;
    }

    public static string GetNameSpaceNoGlobal(ISymbol? symbol)
    {
        if (symbol is null ||
            symbol.ContainingNamespace.IsGlobalNamespace)
            return "";
        return symbol.ContainingNamespace.ToDisplayString(DisplayFormats.ForFileName);
    }

    public static T GetAttributeValue<T>(GeneratorAttributeSyntaxContext context, string argumentName, T defaultValue)
    {
        foreach (var attribute in context.Attributes)
        {
            foreach (var item in attribute.NamedArguments)
            {
                if (item.Key != argumentName)
                    continue;

                return (T)item.Value.Value!;
            }
        }

        return defaultValue;
    }
    
    public static ReturnKind GetReturnsKind(IMethodSymbol methodSymbol)
    {
        if(methodSymbol.ReturnsVoid)
        {
            return ReturnKind.ReturnsVoid;
        }
        if(methodSymbol.ReturnsByRef || methodSymbol.ReturnsByRefReadonly)
        {
            return ReturnKind.ReturnsRef;
        }
        return ReturnKind.ReturnsType;
    }

    public static string[] GetTypeHierarchy(INamedTypeSymbol symbol)
    {
        var nestLevel = CountTypeNesting(symbol);
        return CreateTypeHierarchyInternal(symbol, nestLevel);

        static string[] CreateTypeHierarchyInternal(INamedTypeSymbol? symbol, int level)
        {
            var count = 1;
            string[] container = new string[level];
            while (symbol is not null)
            {
                container[^count] = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                symbol = symbol.ContainingType;
                count++;
            }
            return container;
        }

        static int CountTypeNesting(INamedTypeSymbol? symbol)
        {
            var count = 0;
            while (symbol is not null)
            {
                symbol = symbol.ContainingType;
                count++;
            }
            return count;
        }
    }

    public static string CreateFileName(string containingType)
    {
        return $"{containingType}.g.cs";
    }

    public static string WithModifiers(string typeName, RefKind refKind, bool isNullable)
    {
        switch (refKind)
        {
            case RefKind.Ref:
                return $"ref {typeName}{GetNullableModifier(isNullable)}";
            case RefKind.Out:
                return $"out {typeName}{GetNullableModifier(isNullable)}";
            case RefKind.In:
                return $"in {typeName}{GetNullableModifier(isNullable)}";
            case RefKind.RefReadOnlyParameter:
                return $"ref readonly {typeName}{GetNullableModifier(isNullable)}";
            default:
                if (!isNullable)
                {
                    return typeName;
                }
                return $"{typeName}?";
        }
    }

    private static string GetNullableModifier(bool isNullable)
    {
        return isNullable ? "?" : "";
    }
}

