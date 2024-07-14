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

        internal static MethodInfo CreateDerivedData(
            bool isStatic = true,
            string methodName = "Format",
            ParameterInfo[]? parameters = null,
            ReturnKind returnsKind = ReturnKind.ReturnsType,
            string returnType = "object",
            string spanArgumentType = "int",
            List<string>? typeArguments = null,
            List<TypeConstrainInfo>? typeConstraints = null)
        {
            return new MethodInfo
            {
                IsStatic = isStatic,
                MethodName = methodName,
                Parameters = parameters ??
                [
                    new ParameterInfo("string", "foo", RefKind.Ref, true),
                    new ParameterInfo("int", "baz", RefKind.Out, false),
                    new ParameterInfo("ReadOnlySpan<T>", "args", RefKind.None, false),
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
        MethodInfo? derivedData = null,
        int maxOverrides = 5,
        bool hasParams = true)
        {
            return new SuccessfulParamsCandidate
            {
                TypeInfo = typeInfo ?? CreateCandidateTypeInfo(),
                MethodInfo = derivedData ?? CreateDerivedData(),
                MaxOverrides = maxOverrides,
                HasParams = hasParams
            };
        }
    }
}