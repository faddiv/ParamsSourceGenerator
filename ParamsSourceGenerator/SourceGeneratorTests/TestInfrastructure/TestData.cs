using Foxy.Params.SourceGenerator.Data;
using Foxy.Params.SourceGenerator.SourceGenerator;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace SourceGeneratorTests.TestInfrastructure
{
    internal static class TestData
    {
        internal static CandidateTypeInfo CreateCandidateTypeInfo(
        bool inGlobalNamespace = true,
        string namespaceValue = "Something",
        string[]? typeHierarchy = null,
        string typeName = "Baz")
        {
            return new CandidateTypeInfo
            {
                InGlobalNamespace = inGlobalNamespace,
                Namespace = namespaceValue,
                TypeHierarchy = typeHierarchy ?? ["Foo", "Bar"],
                TypeName = typeName
            };
        }

        internal static DerivedData CreateDerivedData(
            string argName = "foo",
            string argNameSpan = "fooSpan",
            string argNameSpanInput = "fooInput",
            List<string>? fixArguments = null,
            bool isStatic = true,
            string methodName = "Format",
            List<ParameterInfo>? parameterInfos = null,
            ReturnKind returnsKind = ReturnKind.ReturnsType,
            string returnType = "object",
            string spanArgumentType = "int",
            List<string>? typeArguments = null,
            List<TypeConstrainInfo>? typeConstraints = null)
        {
            return new DerivedData
            {
                ArgName = argName,
                ArgNameSpan = argNameSpan,
                ArgNameSpanInput = argNameSpanInput,
                FixArguments = fixArguments ?? ["format", "index"],
                IsStatic = isStatic,
                MethodName = methodName,
                ParameterInfos = parameterInfos ??
                [
                    new ParameterInfo("string", "foo", RefKind.Ref, true),
                new ParameterInfo("int", "baz", RefKind.Out, false),
            ],
                ReturnsKind = returnsKind,
                ReturnType = returnType,
                SpanArgumentType = spanArgumentType,
                TypeArguments = typeArguments ?? ["T1", "T2"],
                TypeConstraints = typeConstraints ??
                [
                    new TypeConstrainInfo
                {
                    Type = "T1",
                    Constraints = ["class", "new()"]
                }
                ]
            };
        }

        internal static SuccessfulParamsCandidate CreateSuccessfulParamsCandidate(
        CandidateTypeInfo? typeInfo = null,
        DerivedData? derivedData = null,
        int maxOverrides = 5,
        bool hasParams = true)
        {
            return new SuccessfulParamsCandidate
            {
                TypeInfo = typeInfo ?? CreateCandidateTypeInfo(),
                DerivedData = derivedData ?? CreateDerivedData(),
                MaxOverrides = maxOverrides,
                HasParams = hasParams
            };
        }
    }
}