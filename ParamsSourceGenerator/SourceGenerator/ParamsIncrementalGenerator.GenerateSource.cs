using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Foxy.Params.SourceGenerator.Helpers;
using Foxy.Params.SourceGenerator.Data;
using Foxy.Params.SourceGenerator.SourceGenerator;

namespace Foxy.Params.SourceGenerator;

partial class ParamsIncrementalGenerator : IIncrementalGenerator
{
    private static void GenerateSource(SourceProductionContext context, ImmutableArray<ParamsCandidate> typeSymbols)
    {
        foreach (var diagnostic in typeSymbols
            .OfType<FailedParamsCandidate>()
            .SelectMany(e => e.Diagnostics))
        {
            context.ReportDiagnostic(diagnostic.ToDiagnostics());
        }

        foreach (var uniqueClass in typeSymbols
            .OfType<SuccessfulParamsCandidate>()
            .GroupBy(e => e.TypeInfo))
        {
            CandidateTypeInfo typeInfo = uniqueClass.Key;
            context.AddSource(
                SemanticHelpers.CreateFileName(typeInfo.TypeName),
                OverridesGenerator.Execute(typeInfo, uniqueClass));
        }
    }
}

