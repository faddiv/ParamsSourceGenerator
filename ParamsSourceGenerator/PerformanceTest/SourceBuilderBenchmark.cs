﻿using BenchmarkDotNet.Attributes;
using Foxy.Params.SourceGenerator.Helpers;

namespace PerformanceTest;

[MemoryDiagnoser]
public class SourceBuilderBenchmark
{
    private readonly string _argName = "example";
    private readonly string _spanArgumentType = "object";

    [Benchmark]
    public void InterpolatedStringHandler()
    {
        var builder = SourceBuilderPool.Instance.Get();
        try
        {
            builder.AddBlock(static (sb, args) =>
            {
                sb.AppendLine($"var {args._argName}Span = new global::System.ReadOnlySpan<{args._spanArgumentType}>({args._argName});");
            }, (_argName, _spanArgumentType));
        }
        finally
        {
            SourceBuilderPool.Instance.Return(builder);
        }
    }
}
