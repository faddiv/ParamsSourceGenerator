using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Test.Infrastructure;
public class SourceGeneratorRunner<T>() : SourceGeneratorRunner([typeof(T)]);

public class SourceGeneratorRunner
{
    private CSharpGeneratorDriver _driver;

    protected SourceGeneratorRunner(
        ImmutableArray<Type> sourceGeneratorTypes)
    {
        var sourceGenerators = CreateSourceGenerators(sourceGeneratorTypes);

        // ⚠ Tell the driver to track all the incremental generator outputs
        // without this, you'll have no tracked outputs!
        var opts = new GeneratorDriverOptions(
            disabledOutputs: IncrementalGeneratorOutputKind.None,
            trackIncrementalGeneratorSteps: true);
        _driver = CSharpGeneratorDriver.Create(sourceGenerators, driverOptions: opts);
    }

    public GeneratorDriverRunResult RunSourceGenerator(
        CSharpCompilation compilation,
        CancellationToken cancellation = default)
    {
        _driver = (CSharpGeneratorDriver)_driver.RunGenerators(compilation, cancellation);
        return _driver.GetRunResult();
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
