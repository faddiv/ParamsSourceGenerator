using BenchmarkDotNet.Attributes;
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
            using (builder.StartBlock())
            {
                builder.AppendLine(
                    $"var {_argName}Span = new global::System.ReadOnlySpan<{_spanArgumentType}>({_argName});");
            }
        }
        finally
        {
            SourceBuilderPool.Instance.Return(builder);
        }
    }
}
