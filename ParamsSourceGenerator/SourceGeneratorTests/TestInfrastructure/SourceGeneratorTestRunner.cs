using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.CodeAnalysis.Testing;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SourceGeneratorTests.TestInfrastructure;

public class SourceGeneratorTestRunner<T>(ReferenceAssemblies? referenceAssemblies = null)
    : SourceGeneratorTestRunner([typeof(T)], referenceAssemblies);

public class SourceGeneratorTestRunner
{
    private readonly ReferenceAssemblies _referenceAssemblies;
    private CSharpGeneratorDriver _driver;
    private readonly MetadataReferenceCollection _references;
    private ImmutableArray<ISourceGenerator> _sourceGenerators;

    public SourceGeneratorTestRunner(
        ImmutableArray<Type> sourceGeneratorTypes,
        ReferenceAssemblies? referenceAssemblies = null)
    {
        _referenceAssemblies = referenceAssemblies ?? ReferenceAssemblies.Net.Net80;
        _sourceGenerators = CreateSourceGenerators(sourceGeneratorTypes);
        _references = [];

        // ⚠ Tell the driver to track all the incremental generator outputs
        // without this, you'll have no tracked outputs!
        var opts = new GeneratorDriverOptions(
            disabledOutputs: IncrementalGeneratorOutputKind.None,
            trackIncrementalGeneratorSteps: true);
        _driver = CSharpGeneratorDriver.Create(_sourceGenerators, driverOptions: opts);
    }

    public string AssemblyName { get; set; } = "TestingAssambly";

    public async Task LoadCSharpAssemblies(
        CancellationToken cancellation = default)
    {
        _references.AddRange(await _referenceAssemblies.ResolveAsync("csharp", cancellation));
    }

    public SourceGeneratorTestRunner AddAdditionalReference(Assembly assembly)
    {
        _references.Add(assembly);
        return this;
    }

    public CSharpCompilation CompileSourceTexts(params string[] sourceTexts)
    {
        EnsureReferencesLoaded();

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

    public CSharpCompilation CompileSources(params CSharpFile[] sources)
    {
        EnsureReferencesLoaded();

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

    public GeneratorDriverRunResult RunSourceGenerator(Compilation compilation, CancellationToken cancellation = default)
    {
        _driver = (CSharpGeneratorDriver)_driver.RunGenerators(compilation, cancellation);
        return _driver.GetRunResult();
    }

    private void EnsureReferencesLoaded()
    {
        if (_references.Count == 0)
        {
            throw new Exception("No references are loaded.");
        }
    }

    private static ImmutableArray<ISourceGenerator> CreateSourceGenerators(
        ImmutableArray<Type> sourceGeneratorTypes)
    {
        return ImmutableArray.CreateRange(sourceGeneratorTypes, static type =>
        {
            object value = Activator.CreateInstance(type)!;
            return value as ISourceGenerator
                ?? ((IIncrementalGenerator)value).AsSourceGenerator();
        });
    }
}