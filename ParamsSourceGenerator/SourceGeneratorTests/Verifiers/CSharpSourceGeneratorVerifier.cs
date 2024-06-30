using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using SourceGeneratorTests.Verifiers;

namespace SourceGeneratorTests;

internal static partial class CSharpSourceGeneratorVerifier<TSourceGenerator>
    where TSourceGenerator : IIncrementalGenerator, new()
{
    private sealed class Test<TSourceGenerator2> : CSharpSourceGeneratorTest<TSourceGenerator2, DefaultVerifier>
        where TSourceGenerator2 : IIncrementalGenerator, new()
    {
        public Test()
        {
        }

        public LanguageVersion LanguageVersion { get; set; } = LanguageVersion.Default;

        protected override CompilationOptions CreateCompilationOptions()
        {
            base.CreateCompilationOptions();
            CompilationOptions compilationOptions = base.CreateCompilationOptions();
            return compilationOptions.WithSpecificDiagnosticOptions(
                 compilationOptions.SpecificDiagnosticOptions.SetItems(CSharpVerifierHelper.NullableWarnings));
        }

        protected override ParseOptions CreateParseOptions()
        {
            return ((CSharpParseOptions)base.CreateParseOptions()).WithLanguageVersion(LanguageVersion);
        }
    }
}
