using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Foxy.Params.SourceGenerator.Helpers;

internal static class Validators
{

    public static bool HasDuplication(ImmutableArray<IParameterSymbol> parameters)
    {
        for (int i = 0; i < parameters.Length; i++)
        {
            for (int j = i + 1; j < parameters.Length; j++)
            {
                if (parameters[i].Name == parameters[j].Name)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static bool HasErrorType(IMethodSymbol methodSymbol)
    {
        foreach (var parameter in methodSymbol.Parameters)
        {
            if (parameter.Type.Kind == SymbolKind.ErrorType)
            {
                return true;
            }
        }

        if (methodSymbol.ReturnType.Kind == SymbolKind.ErrorType)
        {
            return true;

        }

        return false;
    }

    public static bool HasNameCollision(
        ImmutableArray<IParameterSymbol> parameters,
        int maxOverrides,
        [NotNullWhen(true)] out string? unusableParameters)
    {
        unusableParameters = null;
        if (parameters.Length <= 1)
        {
            return false;
        }

        var spanParameterName = parameters[^1].Name;
        var collisionParameters = new List<string>
        {
            $"{spanParameterName}Span"
        };

        for (int i = 0; i < maxOverrides; i++)
        {
            collisionParameters.Add($"{spanParameterName}{i}");
        }

        for (int i = 0; i < parameters.Length - 1; i++)
        {
            if (collisionParameters.Contains(parameters[i].Name))
            {
                unusableParameters = string.Join(", ", collisionParameters);
                return true;
            }
        }

        return false;
    }

    public static bool IsContainingTypesArePartial(
        SyntaxNode targetNode,
        [NotNullWhen(false)] out string? typeName)
    {
        foreach (var containingType in targetNode.Ancestors().OfType<TypeDeclarationSyntax>())
        {
            if (!containingType.Modifiers.Any(token => token.IsKind(SyntaxKind.PartialKeyword)))
            {
                typeName = containingType.Identifier.Text;
                return false;
            }
        }
        typeName = null;
        return true;
    }

    public static bool IsOutParameter(IParameterSymbol? spanParam)
    {
        return spanParam != null
            && spanParam.RefKind == RefKind.Out;
    }

    public static bool IsReadOnlySpan(INamedTypeSymbol? spanParam)
    {
        return spanParam == null || spanParam.MetadataName == "ReadOnlySpan`1";
    }
}
