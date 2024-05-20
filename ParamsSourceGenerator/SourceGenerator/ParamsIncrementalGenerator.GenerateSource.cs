using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Foxy.Params.SourceGenerator.Helpers;
using Foxy.Params.SourceGenerator.Data;
using Foxy.Params.SourceGenerator.SourceGenerator;

namespace Foxy.Params.SourceGenerator
{
    partial class ParamsIncrementalGenerator : IIncrementalGenerator
    {
        private static void GenerateSource(SourceProductionContext context, ImmutableArray<ParamsCandidate> typeSymbols)
        {
            foreach (var diagnostic in typeSymbols
                .OfType<FailedParamsCandidate>()
                .SelectMany(e => e.Diagnostics))
            {
                context.ReportDiagnostic(diagnostic);

            }
            foreach (var uniqueClass in typeSymbols
                .OfType<SuccessfulParamsCandidate>()
                .GroupBy(e => e.ContainingType, SymbolEqualityComparer.Default))
            {
                var typeInfo = uniqueClass.Key as INamedTypeSymbol;
                SemanticHelpers.AssertNotNull(typeInfo);
                var generator = new OverridesGenerator(typeInfo, uniqueClass);
                context.AddSource(
                    SemanticHelpers.CreateFileName(typeInfo),
                    generator.Execute());
            }
        }
    }
}

