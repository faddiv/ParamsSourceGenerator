using Microsoft.CodeAnalysis;
using Foxy.Params.SourceGenerator.Helpers;
using Foxy.Params.SourceGenerator.Data;
using Foxy.Params.SourceGenerator.SourceGenerator;

namespace Foxy.Params.SourceGenerator;

partial class ParamsIncrementalGenerator : IIncrementalGenerator
{
    private static void GenerateSource(SourceProductionContext context, ParamsCandidate typeSymbols)
    {
        if (typeSymbols is FailedParamsCandidate fail)
        {
            foreach (var diagnostic in fail.Diagnostics)
            {
                context.ReportDiagnostic(diagnostic.ToDiagnostics());
            }
        }
        else if (typeSymbols is SuccessfulParamsGroupCandidate group)
        {
            CandidateTypeInfo typeInfo = group.TypeInfo;
            context.AddSource(
                SemanticHelpers.CreateFileName(typeInfo.TypeName),
                OverridesGenerator.Execute(typeInfo, group.ParamCanditates));
        } else
        {
            string diagnosticMessage = $"Invalid ParamsCanditate: {typeSymbols.GetType().Name}";
            Diagnostic diagnostic = Diagnostic.Create(DiagnosticReports.InternalError, null, diagnosticMessage);
            context.ReportDiagnostic(diagnostic);
        }
    }
}

