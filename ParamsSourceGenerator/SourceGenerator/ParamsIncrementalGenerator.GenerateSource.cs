using Microsoft.CodeAnalysis;
using Foxy.Params.SourceGenerator.Helpers;
using Foxy.Params.SourceGenerator.Data;
using Foxy.Params.SourceGenerator.SourceGenerator;

namespace Foxy.Params.SourceGenerator;

partial class ParamsIncrementalGenerator
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
            using var generator = new OverridesGenerator(typeInfo, group.ParamCanditates);
            context.AddSource(
                SemanticHelpers.CreateFileName(typeInfo.TypeName),
                generator.Execute());
        } else
        {
            string diagnosticMessage = $"Invalid ParamsCandidate: {typeSymbols.GetType().Name}";
            Diagnostic diagnostic = Diagnostic.Create(DiagnosticReports.InternalError, null, diagnosticMessage);
            context.ReportDiagnostic(diagnostic);
        }
    }
}

