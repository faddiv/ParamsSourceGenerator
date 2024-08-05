using BenchmarkDotNet.Attributes;
using Foxy.Params.SourceGenerator.Helpers;
using Microsoft.CodeAnalysis;
using SourceGeneratorTests.TestInfrastructure;

namespace PerformanceTest;

[MemoryDiagnoser]
public class SemanticHelpersBenchmark
{
    private INamedTypeSymbol _containingType = null!;

    [GlobalSetup]
    public void Setup()
    {
        var runner = new CompilerRunner();
        runner.LoadCSharpAssemblies().GetAwaiter().GetResult();
        var paramsAttribute = TestEnvironment.GetParamsAttribute();
        var sourceFile = TestEnvironment.GetNestedSourceFile();
        var compilation = runner.CompileSources(paramsAttribute, sourceFile);
        _containingType = TestEnvironment.FindGamma(compilation.Assembly, "Gamma");
    }
    
    [Benchmark]
    public string[] GetTypeHierarchyV3()
    {
        return SemanticHelpers.GetTypeHierarchy(_containingType);
    }
}
