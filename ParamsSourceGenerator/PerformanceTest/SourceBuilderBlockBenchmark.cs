using BenchmarkDotNet.Attributes;
using Foxy.Params.SourceGenerator.Helpers;

namespace PerformanceTest;

[MemoryDiagnoser]
public class SourceBuilderBlockBenchmark
{
    /*[Benchmark]
    public void EmbeddedMethodBlock()
    {
        var builder = SourceBuilderPool.Instance.Get();
        try
        {
            builder.AddBlock(static (sb, _) =>
            {
                sb.AppendLine($"Simple;");
            }, this);
        }
        finally
        {
            SourceBuilderPool.Instance.Return(builder);
        }
    }*/
    
    [Benchmark]
    public void DisposableBlock()
    {
        var builder = SourceBuilderPool.Instance.Get();
        try
        {
            using (builder.StartBlock())
            {
                builder.AppendLine($"Simple;");
            }
        }
        finally
        {
            SourceBuilderPool.Instance.Return(builder);
        }
    }
}
