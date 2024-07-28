﻿using Foxy.Params.SourceGenerator.Data;
using Foxy.Params.SourceGenerator.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foxy.Params.SourceGenerator.SourceGenerator;

internal static class OverridesGenerator
{
    public static SourceText Execute(CandidateTypeInfo typeInfo, IEnumerable<SuccessfulParams> paramsCandidates)
    {
        var _builder = SourceBuilderPool.Instance.Get();
        try
        {
            var _typeInfo = typeInfo ?? throw new ArgumentNullException(nameof(typeInfo));
            var _paramsCandidates = paramsCandidates ?? throw new ArgumentNullException(nameof(paramsCandidates));
            var _maxOverridesMax = paramsCandidates.Max(e => e.MaxOverrides);

            _builder.AutoGeneratedComment();
            _builder.AppendLine();
            _builder.NullableEnable();
            _builder.AppendLine();
            GenerateNamespace(_builder, _typeInfo, _paramsCandidates, _maxOverridesMax);

            return SourceText.From(_builder.ToString(), Encoding.UTF8);
        }
        finally
        {
            SourceBuilderPool.Instance.Return(_builder);
        }
    }

    private static void GenerateNamespace(
        SourceBuilder builder,
        CandidateTypeInfo _typeInfo,
        IEnumerable<SuccessfulParams> paramsCandidates,
        int maxOverridesMax)
    {
        if (_typeInfo.InGlobalNamespace)
        {
            GenerateNamespaceMembers(builder, (_typeInfo, paramsCandidates, maxOverridesMax));
        }
        else
        {
            string namespaceName = _typeInfo.Namespace;
            builder.AppendLine($"namespace {namespaceName}");
            builder.AddBlock(GenerateNamespaceMembers,
                (_typeInfo, paramsCandidates, maxOverridesMax));
        }
    }

    private static void GenerateNamespaceMembers(
        SourceBuilder builder,
        (CandidateTypeInfo typeInfo,
        IEnumerable<SuccessfulParams> paramsCandidates,
        int maxOverridesMax) args)
    {
        GeneratePartialClass(builder, (args.typeInfo, args.paramsCandidates, 0));
        GenerateArgumentsClasses(builder, args.maxOverridesMax);
    }

    private static void GeneratePartialClass(
        SourceBuilder builder, 
        (CandidateTypeInfo typeInfo, IEnumerable<SuccessfulParams> paramsCandidates, int level) args)
    {
        var (typeInfo, paramsCandidates, level) = args;
        if (level < typeInfo.TypeHierarchy.Length)
        {
            builder.AppendLine($"partial class {typeInfo.TypeHierarchy[level]}");
            builder.AddBlock(GeneratePartialClass, (typeInfo, paramsCandidates, level + 1));
        }
        else
        {
            foreach (var paramsCandidate in paramsCandidates)
            {
                var data = paramsCandidate.MethodInfo;

                for (int n = 1; n <= paramsCandidate.MaxOverrides; n++)
                {
                    if (n > 1)
                    {
                        builder.AppendLine();
                    }

                    var variableArguments = data.GetFixArguments().Concat(
                        Enumerable.Range(0, n).Select(j => $"{data.SpanArgumentType} {data.GetArgName()}{j}"));
                    GenerateMethodHeaderWithArguments(builder, data, variableArguments);
                    builder.AddBlock(GenerateBodyForOverrideWithNArgs, (data, n));
                }

                if (paramsCandidate.HasParams)
                {
                    builder.AppendLine();
                    var paramsArguments = data.GetFixArguments()
                        .Append($"params {data.SpanArgumentType}[] {data.GetArgName()}");
                    GenerateMethodHeaderWithArguments(builder, data, paramsArguments);
                    builder.AddBlock(GenerateBodyWithParamsParameter, data);
                }
            }
        }
    }

    private static void GenerateMethodHeaderWithArguments(
        SourceBuilder builder,
        MethodInfo data,
        IEnumerable<string> arguments)
    {
        var line = builder.StartLine();
        line.AddSegment("public");
        if (data.IsStatic)
        {
            line.AddSegment(" static");

        }
        line.AddFormatted($" {data.ReturnType} {data.MethodName}");
        if (data.TypeArguments.Count > 0)
        {
            line.AddFormatted($"<{data.TypeArguments}>");
        }
        line.AddFormatted($"({arguments})");
        line.FinishLine();
        if (data.TypeConstraints.Count <= 0)
            return;
        builder.AddIndented(static (builder, args) =>
        {
            foreach (var typeConstraints in args)
            {
                builder.AppendLine($"where {typeConstraints.Type} : {typeConstraints.Constraints}");
            }
        }, data.TypeConstraints);
    }

    private static void GenerateBodyForOverrideWithNArgs(
        SourceBuilder builder,
        (MethodInfo data, int argsCount) args)
    {
        GenerateArgumentsVariable(builder, args.data, args.argsCount);
        GenerateSpanVariableForInlineArray(builder, args.data, args.argsCount);
        GenerateCallOriginalMethod(builder, args.data);
    }

    private static void GenerateSpanVariableForInlineArray(SourceBuilder builder, MethodInfo data, int argsCount)
    {
        builder.AppendLine(
            $"var {data.GetArgName()}Span = " +
            $"global::System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpan(ref {data.GetArgName()}.arg0, {argsCount});");
    }

    private static void GenerateArgumentsVariable(SourceBuilder builder, MethodInfo data, int argsCount)
    {
        builder.AppendLine(
            $"var {data.GetArgName()} = new Arguments{argsCount}<{data.SpanArgumentType}>" +
            $"({Enumerable.Range(0, argsCount).Select(j => $"{data.GetArgName()}{j}")});");
    }

    private static void GenerateBodyWithParamsParameter(SourceBuilder builder, MethodInfo data)
    {
        GenerateSpanVariableForParamsArgument(builder, data);
        GenerateCallOriginalMethod(builder, data);
    }

    private static void GenerateSpanVariableForParamsArgument(SourceBuilder builder, MethodInfo data)
    {
        builder.AppendLine(
            $"var {data.GetArgName()}Span = new global::System.ReadOnlySpan" +
            $"<{data.SpanArgumentType}>({data.GetArgName()});");
    }

    private static void AddArgNameSpan(MethodInfo data, SourceBuilder.SourceLine line)
    {
        line.AddSegment(data.GetArgName());
        line.AddSegment("Span");
    }

    private static void GenerateArgumentsClasses(SourceBuilder builder, int maxOverridesMax)
    {
        for (int i = 1; i <= maxOverridesMax; i++)
        {
            builder.AppendLine();
            CreateArguments(builder, i);
        }
    }

    private static void GenerateCallOriginalMethod(SourceBuilder builder, MethodInfo data)
    {
        var line = builder.StartLine();
        if (data.ReturnsKind != ReturnKind.ReturnsVoid)
        {
            line.AddSegment("return ");
        }
        if (data.ReturnsKind == ReturnKind.ReturnsRef)
        {
            line.AddSegment("ref ");
        }
        line.AddSegment(data.MethodName);
        if (data.TypeArguments.Count > 0)
        {
            line.AddFormatted($"<{data.TypeArguments}>");
        }

        var fixedParameters = data.GetFixedParameters().Select(e => e.ToPassParameter());
        line.AddFormatted($"({fixedParameters}, {data.GetArgNameSpanInput()});");
        line.FinishLine();
    }

    private static void CreateArguments(SourceBuilder sb, int length)
    {
        sb.AppendLine($"[global::System.Runtime.CompilerServices.InlineArray({length})]");
        sb.AppendLine($"file struct Arguments{length}<T>");
        sb.AddBlock(CreateArgumentsMembers, length);

    }

    private static void CreateArgumentsMembers(SourceBuilder sb, int length)
    {
        sb.AppendTextLine("public T arg0;");
        sb.AppendLine();
        sb.AppendLine($"public Arguments{length}({Enumerable.Range(0, length).Select(e => $"T value{e}")})");
        sb.AddBlock(ArgumentsConstructorBody, length);
    }

    private static void ArgumentsConstructorBody(SourceBuilder builder, int length)
    {
        builder.AppendTextLine("arg0 = value0;");
        for (int i = 1; i < length; i++)
        {
            builder.AppendLine($"this[{i}] = value{i};");
        }
    }
}
