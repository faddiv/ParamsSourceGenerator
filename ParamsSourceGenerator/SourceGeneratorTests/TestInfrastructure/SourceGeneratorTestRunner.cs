using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.CodeAnalysis.Testing;
using System.Collections.Generic;
using System.Linq;

namespace SourceGeneratorTests.TestInfrastructure;

public class SourceGeneratorTestRunner<T>(ReferenceAssemblies referenceAssemblies = null)
    : SourceGeneratorTestRunner([typeof(T)], referenceAssemblies);

public class SourceGeneratorTestRunner
{
    private ReferenceAssemblies _referenceAssemblies;
    private CSharpGeneratorDriver _driver;
    private ImmutableArray<MetadataReference> _references;
    private ImmutableArray<ISourceGenerator> _sourceGenerators;

    public SourceGeneratorTestRunner(
        ImmutableArray<Type> sourceGeneratorTypes,
        ReferenceAssemblies referenceAssemblies = null)
    {
        _referenceAssemblies = referenceAssemblies ?? ReferenceAssemblies.Net.Net80;
        _sourceGenerators = CreateSourceGenerators(sourceGeneratorTypes);

        // ⚠ Tell the driver to track all the incremental generator outputs
        // without this, you'll have no tracked outputs!
        var opts = new GeneratorDriverOptions(
            disabledOutputs: IncrementalGeneratorOutputKind.None,
            trackIncrementalGeneratorSteps: true);
        StartNewGenerator(opts);
    }

    private void StartNewGenerator(GeneratorDriverOptions opts)
    {
        _driver = CSharpGeneratorDriver.Create(_sourceGenerators, driverOptions: opts);
    }

    public string AssemblyName { get; set; } = "TestingAssambly";

    public async Task LoadCSharpAssemblies(CancellationToken cancellation = default)
    {
        _references = await _referenceAssemblies.ResolveAsync("csharp", cancellation);
    }

    public CSharpCompilation CompileSourceTexts(params string[] sourceTexts)
    {
        if (_references.Length == 0)
        {
            throw new Exception("No references are loaded.");
        }

        // Convert the source files to SyntaxTrees
        IEnumerable<SyntaxTree> syntaxTrees = sourceTexts.Select(static x => CSharpSyntaxTree.ParseText(x));

        // Create a Compilation object
        // You may want to specify other results here
        CSharpCompilation compilation = CSharpCompilation.Create(
            AssemblyName,
            syntaxTrees,
            _references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        return compilation;
    }

    public GeneratorDriverRunResult RunSourceGenerator(Compilation compilation, CancellationToken cancellation = default)
    {
        _driver = _driver.RunGenerators(compilation, cancellation) as CSharpGeneratorDriver;
        return _driver.GetRunResult();
    }

    private static ImmutableArray<ISourceGenerator> CreateSourceGenerators(
        ImmutableArray<Type> sourceGeneratorTypes)
    {
        return ImmutableArray.CreateRange(sourceGeneratorTypes, static type =>
        {
            object value = Activator.CreateInstance(type)!;
            return value as ISourceGenerator
                ?? (value as IIncrementalGenerator).AsSourceGenerator();
        });
    }
}