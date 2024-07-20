using Foxy.Params.SourceGenerator.Helpers;
using Foxy.Params.SourceGenerator.NewData;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Immutable;

namespace Foxy.Params.SourceGenerator.SymbolProcessors;

internal static class SymbolToElementVisitors
{
    public static SymbolToElementVisitor MainVisitor { get; } = new();
}

internal class SymbolToElementVisitor : SymbolVisitor<IElement>
{
    public override IElement? VisitMethod(IMethodSymbol symbol)
    {
        var @class = symbol.ContainingType.Accept(this).As<ClassTypeElement>();
        var genericArguments = GetTypeArguments(symbol.TypeArguments, symbol.TypeParameters);
        var @return = GetReturnType(symbol);
        var parameters = GetParameters(symbol.Parameters);
        return new MethodElement(
            name: symbol.Name,
            parent: @class,
            genericArguments: genericArguments,
            @return: @return,
            parameters: parameters
            );
    }

    public override IElement? VisitNamedType(INamedTypeSymbol symbol)
    {
        if (symbol.SpecialType is not SpecialType.None)
        {
            return new KeywordTypeElement(symbol.ToString());
        }
        
        var parent = symbol.ContainingSymbol.Accept(this).As<ITypeOrNamesapce>();
        var genericArguments = GetTypeArguments(symbol.TypeArguments, symbol.TypeParameters);
        return new ClassTypeElement(
            name: symbol.Name,
            parent: parent,
            genericArguments: genericArguments);
    }

    public override IElement? VisitNamespace(INamespaceSymbol symbol)
    {
        if (symbol.IsGlobalNamespace)
        {
            return GlobalNamespaceElement.Instance;
        }

        var parent = symbol.ContainingSymbol.Accept(this).As<INamesapce>();
        return new NamespaceElement(
            name: symbol.Name,
            parent: parent);

    }

    public override IElement? VisitTypeParameter(ITypeParameterSymbol symbol)
    {
        return new OpenGeneric(
            symbol.Name,
            typeConstraints: GetTypes(symbol.ConstraintTypes),
            hasValueTypeConstraint: symbol.HasValueTypeConstraint,
            hasUnmanagedTypeConstraint: symbol.HasUnmanagedTypeConstraint,
            hasNotNullConstraint: symbol.HasNotNullConstraint,
            hasConstructorConstraint: symbol.HasConstructorConstraint);
    }

    public override IElement? VisitParameter(IParameterSymbol symbol)
    {
        return new ParameterElement(
            name: symbol.Name,
            type: symbol.Type.Accept(this).As<ITypeElement>(),
            modifier: GetModifier(symbol.RefKind),
            isNullable: symbol.NullableAnnotation == NullableAnnotation.Annotated);
    }

    private ParameterModifier GetModifier(RefKind refKind)
    {
        return refKind switch
        {
            RefKind.None => ParameterModifier.None,
            RefKind.Ref => ParameterModifier.Ref,
            RefKind.RefReadOnlyParameter => ParameterModifier.Ref,
            RefKind.Out => ParameterModifier.Out,
            RefKind.In => ParameterModifier.In,
            _ => throw new NotImplementedException($"{refKind} is not implemented.")
        };
    }

    public override IElement? DefaultVisit(ISymbol symbol)
    {
        return base.DefaultVisit(symbol);
    }

    private ParameterElement[]? GetParameters(in ImmutableArray<IParameterSymbol> parameters)
    {
        if (parameters.Length <= 0)
        {
            return [];
        }
        var p = new ParameterElement[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            p[i] = parameters[i].Accept(this).As<ParameterElement>();
        }
        return p;
    }

    private IReturnElement GetReturnType(IMethodSymbol symbol)
    {
        var kind = SemanticHelpers.GetReturnsKind(symbol);
        return kind switch
        {
            Data.ReturnKind.ReturnsVoid => VoidReturnElement.Instance,
            Data.ReturnKind.ReturnsType
                => new TypedReturnElement(symbol.ReturnType.Accept(this).As<ITypeElement>()),
            Data.ReturnKind.ReturnsRef
                => new TypedReturnElement(symbol.ReturnType.Accept(this).As<ITypeElement>(), ReturnKindEnum.Ref),
            _ => throw new ArgumentException($"Invalid return kind: {kind}"),
        };
    }

    private IGenericElement[] GetTypeArguments(
        in ImmutableArray<ITypeSymbol> typeArguments,
        in ImmutableArray<ITypeParameterSymbol> typeParameters)
    {
        if (typeArguments.Length <= 0)
        {
            return GetTypeArguments(typeParameters);
        }

        var elements = new IGenericElement[typeArguments.Length];
        for (int i = 0; i < typeArguments.Length; i++)
        {
            elements[i] = new ClosedGeneric(typeArguments[i].Accept(this).As<ITypeElement>());
        }

        return elements;
    }

    private IGenericElement[] GetTypeArguments(
        in ImmutableArray<ITypeParameterSymbol> typeParameters)
    {
        if (typeParameters.Length <= 0)
        {
            return [];
        }

        var elements = new IGenericElement[typeParameters.Length];
        for (int i = 0; i < typeParameters.Length; i++)
        {
            elements[i] = typeParameters[i].Accept(this).As<OpenGeneric>();
        }

        return elements;
    }

    private ITypeElement[] GetTypes(in ImmutableArray<ITypeSymbol> typeSymbols)
    {
        if (typeSymbols.Length <= 0)
        {
            return [];
        }
        var typeElements = new ITypeElement[typeSymbols.Length];
        for (int i = 0; i < typeSymbols.Length; i++)
        {
            typeElements[i] = typeSymbols[i].Accept(this).As<ITypeElement>();
        }
        return typeElements;
    }

}