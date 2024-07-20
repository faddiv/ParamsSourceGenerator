using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using Foxy.Params.SourceGenerator.Helpers;
using Foxy.Params.SourceGenerator.Data;
using System.Buffers;

namespace Foxy.Params.SourceGenerator;

partial class ParamsIncrementalGenerator : IIncrementalGenerator
{
    private ParamsCandidate? GetSpanParamsMethods(
        GeneratorAttributeSyntaxContext context,
        CancellationToken cancellationToken)
    {
        SyntaxNode targetNode = context.TargetNode;
        Debug.Assert(targetNode is MethodDeclarationSyntax);
        var decl = Unsafe.As<MethodDeclarationSyntax>(targetNode);

        if (!(context.SemanticModel.GetDeclaredSymbol(decl, cancellationToken) is IMethodSymbol methodSymbol)
            || !SemanticHelpers.TryGetAttribute(
                decl, _attributeName, context.SemanticModel, cancellationToken, out var attributeSyntax))
        {
            return null;
        }

        if (Validators.HasErrorType(methodSymbol) ||
            Validators.HasDuplication(methodSymbol.Parameters))
        {
            return null;
        }

        var diagnostics = new List<DiagnosticInfo>();
        if (!Validators.IsContainingTypesArePartial(targetNode, out var typeName))
        {
            diagnostics.Add(DiagnosticInfo.Create(
                DiagnosticReports.PartialIsMissingDescriptor,
                attributeSyntax.GetLocation(),
                typeName,
                methodSymbol.Name));
        }

        int maxOverrides = SemanticHelpers.GetValue(context.Attributes.First(), "MaxOverrides", 3);
        var spanParam = methodSymbol.Parameters.LastOrDefault();
        if (spanParam is null ||
            spanParam?.Type is not INamedTypeSymbol spanType)
        {
            diagnostics.Add(DiagnosticInfo.Create(
                DiagnosticReports.ParameterMissingDescriptor,
                attributeSyntax.GetLocation(),
                methodSymbol.Name));
        }
        else if (!Validators.IsReadOnlySpan(spanType))
        {
            diagnostics.Add(DiagnosticInfo.Create(
                DiagnosticReports.ParameterMismatchDescriptor,
                attributeSyntax.GetLocation(),
                methodSymbol.Name, spanParam.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));
        }
        else if (Validators.IsOutParameter(spanParam))
        {
            diagnostics.Add(DiagnosticInfo.Create(
                DiagnosticReports.OutModifierNotAllowedDescriptor,
                attributeSyntax.GetLocation(),
                methodSymbol.Name, spanParam.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));
        }

        if (Validators.HasNameCollision(methodSymbol.Parameters, maxOverrides, out string? unusableParameters))
        {
            diagnostics.Add(DiagnosticInfo.Create(
                DiagnosticReports.ParameterCollisionDescriptor,
                attributeSyntax.GetLocation(),
                methodSymbol.Name, unusableParameters));
        }

        if (diagnostics.Count > 0)
        {
            return new FailedParamsCandidate { Diagnostics = diagnostics };
        }
        INamedTypeSymbol containingType = methodSymbol.ContainingType;
        var parameterInfos = MethodInfo.GetArguments(methodSymbol);
        return new SuccessfulParamsCandidate
        {
            TypeInfo = new CandidateTypeInfo
            { 
                TypeName = containingType.ToDisplayString(DisplayFormats.ForFileName),
                TypeHierarchy = SemanticHelpers.GetTypeHierarchy(containingType)
                    .Select(e => e.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat))
                    .ToArray(),
                InGlobalNamespace = containingType.ContainingNamespace.IsGlobalNamespace,
                Namespace = SemanticHelpers.GetNameSpaceNoGlobal(containingType)
            },
            MaxOverrides = maxOverrides,
            HasParams = SemanticHelpers.GetValue(context.Attributes.First(), "HasParams", true),
            MethodInfo = new MethodInfo
            {
                ReturnType = MethodInfo.CreateReturnTypeFor(methodSymbol),
                Parameters = parameterInfos,
                ReturnsKind = SemanticHelpers.GetReturnsKind(methodSymbol),
                TypeArguments = methodSymbol.TypeArguments
                    .Select(e => e.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat))
                    .ToList(),
                TypeConstraints = MethodInfo.CreateTypeConstraints(methodSymbol.TypeArguments),
                MethodName = methodSymbol.Name,
                IsStatic = methodSymbol.IsStatic
            }
        };
    }
}

