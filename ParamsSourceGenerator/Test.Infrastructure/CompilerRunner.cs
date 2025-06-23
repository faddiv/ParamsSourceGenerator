using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;

namespace Test.Infrastructure;

public class CompilerRunner(ReferenceAssemblies? referenceAssemblies = null)
{
    private readonly ReferenceAssemblies _referenceAssemblies = referenceAssemblies ?? ReferenceAssemblies.Net.Net80;
    private ImmutableArray<MetadataReference> _references;

    private string AssemblyName { get; set; } = "TestingAssembly";

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
