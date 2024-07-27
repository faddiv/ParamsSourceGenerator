using BenchmarkDotNet.Attributes;
using Foxy.Params.SourceGenerator.Helpers;

namespace PerformanceTest;

[MemoryDiagnoser]
public class SourceBuilderBenchmark
{
    private readonly string _argName = "example";
    private readonly string _spanArgumentType = "object";

    [Benchmark]
    public string StartLineWithAddSegments()
    {
        var builder = new SourceBuilder();
        builder.AddBlock(static (sb, args) =>
        {
            var line = sb.StartLine();
            line.AddSegment("var ");
            line.AddSegment(args._argName);
            line.AddSegment("Span");
            line.AddSegment(" = new global::System.ReadOnlySpan<");
            line.AddSegment(args._spanArgumentType);
            line.AddSegment(">(");
            line.AddSegment(args._argName);
            line.AddSegment(")");
            line.EndLine();
        }, (_argName, _spanArgumentType));
        return builder.ToString();
    }

    [Benchmark]
    public string AppendLineWithStringInterpolation()
    {
        var builder = new SourceBuilder();
        builder.AddBlock(static (sb, args) =>
        {
            sb.AppendLineV1($"var {args._argName}Span = new global::System.ReadOnlySpan<{args._spanArgumentType}>({args._argName});");
        }, (_argName, _spanArgumentType));
        return builder.ToString();
    }

    [Benchmark]
    public string PassStringInterpolationToBuilder()
    {
        var builder = new SourceBuilder();
        builder.AddBlock(static (sb, args) =>
        {
            sb.AppendV2($"var {args._argName}Span = new global::System.ReadOnlySpan<{args._spanArgumentType}>({args._argName});");
        }, (_argName, _spanArgumentType));
        return builder.ToString();
    }

    [Benchmark]
    public string InterpolatedStringHandler()
    {
        var builder = new SourceBuilder();
        builder.AddBlock(static (sb, args) =>
        {
            sb.AppendV3($"var {args._argName}Span = new global::System.ReadOnlySpan<{args._spanArgumentType}>({args._argName});");
        }, (_argName, _spanArgumentType));
        return builder.ToString();
    }
}
