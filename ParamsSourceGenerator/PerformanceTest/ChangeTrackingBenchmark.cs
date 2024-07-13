using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Foxy.Params.SourceGenerator;
using Microsoft.CodeAnalysis.Testing;

namespace PerformanceTest;

[MemoryDiagnoser]
public class ChangeTrackingBenchmark
{
    private CSharpCompilation _compilation1 = null!;
    private CSharpCompilation _compilation2 = null!;
    private GeneratorDriver _driver = null!;

    [GlobalSetup]
    public async Task CreateBaseSource()
    {
        string code1 = """
using System;
using Foxy.Params;
using System.Collections.Generic;

namespace Something;

public partial class Foo{0}
{{
    [Params(MaxOverrides = 10)]
    private static void Format(string? format, List<string> sources, ReadOnlySpan<object> args)
    {{
    }}
}}
""";
        string code2 = """
using System;
using Foxy.Params;
using System.Collections.Generic;

namespace Something;

public partial class Foo0
{
    [Params(MaxOverrides = 10)]
    private static void Format(int? format, List<string> sources, ReadOnlySpan<object> args)
    {
    }
}
""";
        var sources = new List<SyntaxTree>(1000);
        for (int i = 0; i < 1000; i++)
        {
            var name = $"Foo{i}.cs";
            var code = string.Format(code1, i);
            var tree = CSharpSyntaxTree.ParseText(code, path: name);
            sources.Add(tree);
        }
        var tree2 = CSharpSyntaxTree.ParseText(code2, path: $"Foo0.cs");
        

        var references = await ReferenceAssemblies.Net.Net80.ResolveAsync("csharp", CancellationToken.None);
        _compilation1 = CSharpCompilation.Create("foos", sources, references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        _compilation2 = _compilation1.ReplaceSyntaxTree(sources[0], tree2);

        var generator = new ParamsIncrementalGenerator();
        var sourceGenerators = new List<ISourceGenerator> { generator.AsSourceGenerator() };
        var opts = new GeneratorDriverOptions(
            disabledOutputs: IncrementalGeneratorOutputKind.None,
            trackIncrementalGeneratorSteps: false);

        _driver = CSharpGeneratorDriver.Create(sourceGenerators, driverOptions: opts);

    }
    [Benchmark]
    public void OnlyOneFileChanges()
    {
        _driver = _driver.RunGenerators(_compilation1);
        (_compilation1, _compilation2) = (_compilation2, _compilation1);
        /*var output = _driver.GetRunResult();
        Console.WriteLine(output);*/
    }
}
