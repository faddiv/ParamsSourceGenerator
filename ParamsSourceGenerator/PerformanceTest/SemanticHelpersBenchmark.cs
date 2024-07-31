using BenchmarkDotNet.Attributes;
using Foxy.Params.SourceGenerator.Helpers;
using Microsoft.CodeAnalysis;
using PerformanceTest.Helpers;

namespace PerformanceTest;

[MemoryDiagnoser]
public class SemanticHelpersBenchmark
{
    private NamedTypeSymbol _containingType = null!;

    [GlobalSetup]
    public void Setup()
    {
        _containingType = new NamedTypeSymbol
        {
            Name = "gamma",
            ContainingType = new NamedTypeSymbol
            {
                Name = "beta",
                ContainingType = new NamedTypeSymbol
                {
                    Name = "alfa"
                }
            }
        };
    }
    
    [Benchmark]
    public string[] GetTypeHierarchyV3()
    {
        return SemanticHelpers.GetTypeHierarchy(_containingType);
    }
}
