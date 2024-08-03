using BenchmarkDotNet.Attributes;
using Foxy.Params.SourceGenerator.Helpers;
using Microsoft.CodeAnalysis;
using PerformanceTest.Helpers;
using SourceGeneratorTests.TestInfrastructure;

namespace PerformanceTest;

[MemoryDiagnoser]
public class SemanticHelpersBenchmark
{
    private INamedTypeSymbol _containingType = null!;

    [GlobalSetup]
    public void Setup()
    {
        var runner = new SourceGeneratorTestRunner();
        runner.LoadCSharpAssemblies().GetAwaiter().GetResult();
        var paramsAttribute = TestEnvironment.GetFile("ParamsAttribute.cs");
        var sourceFile = TestEnvironment.GetFile("SourceFile.cs");
        var compilation = runner.CompileSources(paramsAttribute, sourceFile);
        _containingType = TestEnvironment.FindGamma(compilation.Assembly);
    }
    
    [Benchmark]
    public string[] GetTypeHierarchyV3()
    {
        return SemanticHelpers.GetTypeHierarchy(_containingType);
    }
}
