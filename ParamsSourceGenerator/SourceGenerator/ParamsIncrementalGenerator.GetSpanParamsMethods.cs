using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Foxy.Params.SourceGenerator.Helpers;
using Foxy.Params.SourceGenerator.Data;

namespace Foxy.Params.SourceGenerator;

partial class ParamsIncrementalGenerator : IIncrementalGenerator
{
    private ParamsCandidate? GetSpanParamsMethods(
        GeneratorAttributeSyntaxContext context,
        CancellationToken cancel)
    {
        if (context.TargetNode is not MethodDeclarationSyntax methodDeclarationSyntax ||
            context.TargetSymbol is not IMethodSymbol methodSymbol)
        {
            return null;
        }

        if (Validators.HasErrorType(methodSymbol) ||
            Validators.HasDuplication(methodSymbol.Parameters))
        {
            return null;
        }
        
        var diagnostics = new List<DiagnosticInfo>();
        if (!Validators.IsContainingTypesArePartial(methodDeclarationSyntax, out var typeName))
        {
            diagnostics.Add(DiagnosticInfo.Create(
                DiagnosticReports.PartialIsMissingDescriptor,
                context.GetAttributeLocation(cancel),
                typeName,
                methodSymbol.Name));
        }

        var spanParam = methodSymbol.Parameters.LastOrDefault();
        if (spanParam is null ||
            spanParam?.Type is not INamedTypeSymbol spanType)
        {
            diagnostics.Add(DiagnosticInfo.Create(
                DiagnosticReports.ParameterMissingDescriptor,
                context.GetAttributeLocation(cancel),
                methodSymbol.Name));
        }
        else if (!Validators.IsReadOnlySpan(spanType))
        {
            diagnostics.Add(DiagnosticInfo.Create(
                DiagnosticReports.ParameterMismatchDescriptor,
                context.GetAttributeLocation(cancel),
                methodSymbol.Name, spanParam.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));
        }
        else if (Validators.IsOutParameter(spanParam))
        {
            diagnostics.Add(DiagnosticInfo.Create(
                DiagnosticReports.OutModifierNotAllowedDescriptor,
                context.GetAttributeLocation(cancel),
                methodSymbol.Name, spanParam.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)));
        }

        bool hasParams = SemanticHelpers.GetAttributeValue(context, "HasParams", true);
        int maxOverrides = SemanticHelpers.GetAttributeValue(context, "MaxOverrides", 3);
        if (Validators.HasNameCollision(methodSymbol.Parameters, maxOverrides, out string? unusableParameters))
        {
            diagnostics.Add(DiagnosticInfo.Create(
                DiagnosticReports.ParameterCollisionDescriptor,
                context.GetAttributeLocation(cancel),
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
                TypeHierarchy = SemanticHelpers.GetTypeHierarchy(containingType),
                InGlobalNamespace = containingType.ContainingNamespace.IsGlobalNamespace,
                Namespace = SemanticHelpers.GetNameSpaceNoGlobal(containingType)
            },
            MaxOverrides = maxOverrides,
            HasParams = hasParams,
            MethodInfo = new MethodInfo
            {
                ReturnType = MethodInfo.CreateReturnTypeFor(methodSymbol),
                Parameters = parameterInfos,
                ReturnsKind = SemanticHelpers.GetReturnsKind(methodSymbol),
                TypeConstraints = MethodInfo.CreateTypeConstraints(methodSymbol.TypeArguments),
                MethodName = methodSymbol.Name,
                IsStatic = methodSymbol.IsStatic
            }
        };
    }
}

