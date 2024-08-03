using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using SourceGeneratorTests.TestInfrastructure;

namespace PerformanceTest.Helpers;

public class SourceGeneratorTestRunner
{
    private readonly ReferenceAssemblies _referenceAssemblies;
    private ImmutableArray<MetadataReference> _references;

    public SourceGeneratorTestRunner(
        ReferenceAssemblies? referenceAssemblies = null)
    {
        _referenceAssemblies = referenceAssemblies ?? ReferenceAssemblies.Net.Net80;

        // ⚠ Tell the driver to track all the incremental generator outputs
        // without this, you'll have no tracked outputs!
        var opts = new GeneratorDriverOptions(
            disabledOutputs: IncrementalGeneratorOutputKind.None,
            trackIncrementalGeneratorSteps: true);
    }

    public string AssemblyName { get; set; } = "TestingAssambly";

    public async Task LoadCSharpAssemblies(CancellationToken cancellation = default)
    {
        _references = await _referenceAssemblies.ResolveAsync("csharp", cancellation);
    }

    public CSharpCompilation CompileSources(params CSharpFile[] sources)
    {
        if (_references.Length == 0)
        {
            throw new Exception("No references are loaded.");
        }

        // Convert the source files to SyntaxTrees
        IEnumerable<SyntaxTree> syntaxTrees = sources.Select(static x => CSharpSyntaxTree.ParseText(x.Content, path: x.Name));

        // Create a Compilation object
        // You may want to specify other results here
        CSharpCompilation compilation = CSharpCompilation.Create(
            AssemblyName,
            syntaxTrees,
            _references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        
        return compilation;
    }
}