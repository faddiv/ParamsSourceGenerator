using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Test.Infrastructure;

namespace SourceGeneratorTests.TestInfrastructure.Verifiers;

partial class CSharpSourceGeneratorVerifier<TSourceGenerator>
{
    public static DiagnosticResult Diagnostic() => new DiagnosticResult();

    public static DiagnosticResult Diagnostic(string id, DiagnosticSeverity severity) => new(id, severity);

    public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor) => new(descriptor);

    public static async Task VerifyGeneratorAsync(
        CSharpFile source,
        CSharpFile generatedSource)
        => await VerifyGeneratorAsync(source, DiagnosticResult.EmptyDiagnosticResults, [generatedSource]);

    public static async Task VerifyGeneratorAsync(
        CSharpFile source,
        params CSharpFile[] generatedSources)
        => await VerifyGeneratorAsync(source, DiagnosticResult.EmptyDiagnosticResults, generatedSources);

    public static async Task VerifyGeneratorAsync(
        CSharpFile source,
        DiagnosticResult diagnostic)
        => await VerifyGeneratorAsync(source, [diagnostic], generatedSources: []);

    public static async Task VerifyGeneratorAsync(
        CSharpFile source,
        params DiagnosticResult[] diagnostics)
        => await VerifyGeneratorAsync(source, diagnostics, generatedSources: []);

    public static async Task VerifyGeneratorAsync(
        CSharpFile source,
        DiagnosticResult diagnostic,
        CSharpFile generatedSource)
        => await VerifyGeneratorAsync(source, [diagnostic], [generatedSource]);

    public static async Task VerifyGeneratorAsync(
        CSharpFile source,
        DiagnosticResult[] diagnostics,
        CSharpFile generatedSource)
        => await VerifyGeneratorAsync(source, diagnostics, [generatedSource]);

    public static async Task VerifyGeneratorAsync(
        CSharpFile source,
        DiagnosticResult diagnostic,
        CSharpFile[] generatedSources)
        => await VerifyGeneratorAsync(source, [diagnostic], generatedSources);

    public static async Task VerifyGeneratorAsync(
        CSharpFile source,
        DiagnosticResult[] diagnostics,
        CSharpFile[] generatedSources,
        CancellationToken cancellation = default)
    {
        Test<TSourceGenerator> test = new()
        {
            TestState =
            {
                Sources = { (source.Name, source.Content) },
            },
            ReferenceAssemblies = ReferenceAssemblies.Net.Net80,
        };

        foreach ((string filename, string content) in generatedSources)
        {
            test.TestState.GeneratedSources.Add((typeof(TSourceGenerator), filename, SourceText.From(content, Encoding.UTF8)));
        }

        test.ExpectedDiagnostics.AddRange(diagnostics);

        await test.RunAsync(cancellation);
    }
}
