using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Foxy.Params.SourceGenerator.Data;
using Foxy.Params.SourceGenerator.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Foxy.Params.SourceGenerator;

[Generator]
public partial class ParamsIncrementalGenerator : IIncrementalGenerator
{
    private const string _attributeName = "Foxy.Params.ParamsAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(AddParamsAttribute);
        var declarations = context.SyntaxProvider.ForAttributeWithMetadataName(
            _attributeName,
            predicate: Filter,
            transform: GetSpanParamsMethodsV2)
            .WithTrackingName(TrackingNames.GetSpanParamsMethods)
            .NotNull()
            .WithTrackingName(TrackingNames.NotNullFilter)
            .Collect()
            .SelectMany(GroupV2);

        context.RegisterSourceOutput(declarations, GenerateSource);
    }

    private static IEnumerable<ParamsCandidate> Group(ImmutableArray<ParamsCandidate> e, CancellationToken c)
    {
        return e.OfType<SuccessfulParamsCandidate>().GroupBy(x => x.TypeInfo).Select(x => new SuccessfulParamsGroupCandidate
        {
            ParamCanditates = x.Select(y => new SuccessfulParams
            {
                MethodInfo = y.MethodInfo,
                HasParams = y.HasParams,
                MaxOverrides = y.MaxOverrides,
            }).ToImmutableList(),
            TypeInfo = x.Key
        }).Cast<ParamsCandidate>()
        .Concat(e.OfType<FailedParamsCandidate>());
    }

    private static IEnumerable<ParamsCandidate> GroupV2(ImmutableArray<ParamsCandidate> e, CancellationToken c)
    {
        return e.OfType<SuccessfulParamsCandidateV2>()
            .GroupBy(x => x.TypeInfo)
            .Select(x => new SuccessfulParamsGroupCandidateV2
            {
                ParamCanditates = x.Select(y => new SuccessfulParamsV2 {
                    HasParams = y.HasParams,
                    MaxOverrides = y.MaxOverrides,
                    MethodInfo = y.MethodInfo
                }).ToImmutableList(),
                TypeInfo = x.Key
            }).Cast<ParamsCandidate>()
        .Concat(e.OfType<FailedParamsCandidate>());
    }

    private void AddParamsAttribute(IncrementalGeneratorPostInitializationContext context)
    {
        context.AddSource("ParamsAttribute.g.cs", _paramsAttribute);
    }

    private static bool Filter(SyntaxNode s, CancellationToken token)
    {
        return s is MethodDeclarationSyntax methodDeclarationSyntax;
    }
}

