using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Foxy.Params.SourceGenerator;
using System.Reflection;

namespace PerformanceTest;

[MemoryDiagnoser]
public class CodeGenerationBenchmark
{
    private CSharpCompilation _compilation = null!;
    private CSharpGeneratorDriver _driver = null!;

    [GlobalSetup]
    public void CreateBaseSource()
    {
        string code = @"using System;
using Foxy.Params;
using System.Collections.Generic;

namespace Something;

public partial class Foo
{
    [Params(MaxOverrides = 10)]
    private static void Format(ref string? format, List<string> sources, ReadOnlySpan<object> args)
    {
    }
}
";
        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        var references = new List<MetadataReference>();
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            if (!assembly.IsDynamic)
            {
                references.Add(MetadataReference.CreateFromFile(assembly.Location));
            }
        }

        _compilation = CSharpCompilation.Create("foo", [syntaxTree], references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var generator = new ParamsIncrementalGenerator();

        _driver = CSharpGeneratorDriver.Create(generator);

    }
    [Benchmark]
    public Compilation RunGenerator()
    {
        _driver.RunGeneratorsAndUpdateCompilation(_compilation, out var outputCompilation, out var generateDiagnostics);

        return outputCompilation;
    }
}
