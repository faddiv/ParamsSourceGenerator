﻿using BenchmarkDotNet.Attributes;
using Foxy.Params.SourceGenerator.Data;
using Foxy.Params.SourceGenerator.Helpers;
using Microsoft.CodeAnalysis;
using PerformanceTest.Helpers;
using Test.Infrastructure;
using MethodInfo = Foxy.Params.SourceGenerator.Data.MethodInfo;

namespace PerformanceTest;

[MemoryDiagnoser]
public class SuccessfulParamsCandidateCreationBenchmark
{
    private IMethodSymbol _methodSymbol = null!;
    private int _maxOverrides = 3;
    private bool _hasParams = true;

    [GlobalSetup]
    public void Setup()
    {
        var runner = new CompilerRunner();
        runner.LoadCSharpAssemblies().GetAwaiter().GetResult();
        var paramsAttribute = TestEnvironment.GetParamsAttribute();
        var sourceFile = TestEnvironment.GetNestedSourceFile();
        var compilation = runner.CompileSources(paramsAttribute, sourceFile);
        _methodSymbol = TestEnvironment.FindMethodByName(compilation.Assembly, "Gamma", "Format");
    }

    [Benchmark]
    public object CreateSuccessfulParamsCandidate()
    {
        INamedTypeSymbol containingType = _methodSymbol.ContainingType;
        var parameterInfos = MethodInfo.GetArguments(_methodSymbol);
        return new SuccessfulParamsCandidate
        {
            TypeInfo = new CandidateTypeInfo
            {
                TypeName = containingType.ToDisplayString(DisplayFormats.ForFileName),
                TypeHierarchy = SemanticHelpers.GetTypeHierarchy(containingType),
                InGlobalNamespace = containingType.ContainingNamespace.IsGlobalNamespace,
                Namespace = SemanticHelpers.GetNameSpaceNoGlobal(containingType)
            },
            MaxOverrides = _maxOverrides,
            HasParams = _hasParams,
            MethodInfo = new MethodInfo
            {
                ReturnType = MethodInfo.CreateReturnTypeFor(_methodSymbol),
                Parameters = parameterInfos,
                ReturnsKind = SemanticHelpers.GetReturnsKind(_methodSymbol),
                TypeConstraints = MethodInfo.CreateTypeConstraints(_methodSymbol.TypeArguments),
                MethodName = _methodSymbol.Name,
                IsStatic = _methodSymbol.IsStatic
            }
        };
    }

    [Benchmark]
    public object CreateMethodInfo()
    {
        var parameterInfos = MethodInfo.GetArguments(_methodSymbol);

        return new MethodInfo
        {
            ReturnType = MethodInfo.CreateReturnTypeFor(_methodSymbol),
            Parameters = parameterInfos,
            ReturnsKind = SemanticHelpers.GetReturnsKind(_methodSymbol),
            TypeConstraints = MethodInfo.CreateTypeConstraints(_methodSymbol.TypeArguments),
            MethodName = _methodSymbol.Name,
            IsStatic = _methodSymbol.IsStatic
        };
    }
}
