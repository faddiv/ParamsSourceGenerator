using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using SourceGeneratorTests.TestInfrastructure;

namespace SourceGeneratorTests;

partial class CSharpSourceGeneratorVerifier<TSourceGenerator>
{
    private static readonly CSharpFile[] _emptyGeneratedSources = [];

    public static DiagnosticResult Diagnostic() => new DiagnosticResult();

    public static DiagnosticResult Diagnostic(string id, DiagnosticSeverity severity) => new(id, severity);

    public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor) => new(descriptor);

    public static async Task VerifyGeneratorAsync(
        string source,
        CSharpFile generatedSource)
        => await VerifyGeneratorAsync(source, DiagnosticResult.EmptyDiagnosticResults, [generatedSource]);

    public static async Task VerifyGeneratorAsync(
        string source,
        params CSharpFile[] generatedSources)
        => await VerifyGeneratorAsync(source, DiagnosticResult.EmptyDiagnosticResults, generatedSources);

    public static async Task VerifyGeneratorAsync(
        string source,
        DiagnosticResult diagnostic)
        => await VerifyGeneratorAsync(source, [diagnostic], _emptyGeneratedSources);

    public static async Task VerifyGeneratorAsync(
        string source,
        params DiagnosticResult[] diagnostics)
        => await VerifyGeneratorAsync(source, diagnostics, _emptyGeneratedSources);

    public static async Task VerifyGeneratorAsync(
        string source,
        DiagnosticResult diagnostic,
        CSharpFile generatedSource)
        => await VerifyGeneratorAsync(source, [diagnostic], [generatedSource]);

    public static async Task VerifyGeneratorAsync(
        string source,
        DiagnosticResult[] diagnostics,
        CSharpFile generatedSource)
        => await VerifyGeneratorAsync(source, diagnostics, [generatedSource]);

    public static async Task VerifyGeneratorAsync(
        string source,
        DiagnosticResult diagnostic,
        CSharpFile[] generatedSources)
        => await VerifyGeneratorAsync(source, [diagnostic], generatedSources);

    public static async Task VerifyGeneratorAsync(
        string source,
        DiagnosticResult[] diagnostics,
        CSharpFile[] generatedSources,
        CancellationToken cancellation = default)
    {
        Test<TSourceGenerator> test = new()
        {
            TestState =
            {
                Sources = { source },
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
