using BenchmarkDotNet.Attributes;
using Foxy.Params.SourceGenerator.Data;
using Foxy.Params.SourceGenerator.Helpers;
using PerformanceTest.Data;

namespace PerformanceTest;

[MemoryDiagnoser]
public class GenericTypeInfoBenchmark
{
    private TypeConstraintsOld[] _typeConstraintsOld = null!;
    private GenericTypeInfo[] _genericTypeInfo = null!;

    [GlobalSetup]
    public void Setup()
    {
        _typeConstraintsOld =
        [
            new TypeConstraintsOld
            {
                Type = "T1",
                Constraints = [
                    "class",
                    "object",
                    "Sample.Namespace.Class1",
                    "Sample.Namespace.Class2",
                    "new()"
                ]
            },
            new TypeConstraintsOld
            {
                Type = "T2",
                Constraints = [
                    "struct",
                    "Tuple",
                    "Sample.Namespace.Struct1",
                    "Sample.Namespace.Struct2",
                    "new()"
                ]
            }
        ];
        _genericTypeInfo =
        [
            new GenericTypeInfo
            {
                Type = "T1",
                ConstraintType = ConstraintType.Class,
                ConstraintTypes = [
                    "Sample.Namespace.Class1",
                    "Sample.Namespace.Class2"
                ],
                HasConstructorConstraint = true
            },
            new GenericTypeInfo
            {
                Type = "T2",
                ConstraintType = ConstraintType.Struct,
                ConstraintTypes = [
                    "Sample.Namespace.Struct1",
                    "Sample.Namespace.Struct2"
                ],
                HasConstructorConstraint = true
            }
        ];
    }

    [Benchmark]
    public void TypeConstraintV1()
    {
        var builder = SourceBuilderPool.Instance.Get();
        try
        {
            using (builder.StartBlock())
            {
                foreach (var arg in _typeConstraintsOld)
                {
                    builder.AppendLine($"where {arg.Type}: {arg.Constraints}");
                }   
            }
        }
        finally
        {
            SourceBuilderPool.Instance.Return(builder);
        }
    }

    [Benchmark]
    public void TypeConstraintV2()
    {
        var builder = SourceBuilderPool.Instance.Get();
        try
        {
            using (builder.StartBlock())
            {
                foreach (var arg in _genericTypeInfo)
                {
                    arg.WriteTo(builder);
                }
                
            }
        }
        finally
        {
            SourceBuilderPool.Instance.Return(builder);
        }
    }

}
