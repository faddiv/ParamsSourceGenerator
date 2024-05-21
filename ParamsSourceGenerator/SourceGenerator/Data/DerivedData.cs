using Foxy.Params.SourceGenerator.Helpers;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Foxy.Params.SourceGenerator.Data
{
    internal class DerivedData
    {
        public DerivedData(SuccessfulParamsCandidate paramsCandidate) {
            Candidate = paramsCandidate;

            ReturnType = CreateReturnTypeFor(paramsCandidate.MethodSymbol);
            SpanArgumentType = GetSpanArgumentType(paramsCandidate.SpanParam);
            ParameterInfos = GetNonParamsArguments(paramsCandidate.MethodSymbol);
            FixArguments = ParameterInfos.Select(e => e.ToParameter()).ToList();
            ReturnsKind = SemanticHelpers.GetReturnsKind(paramsCandidate.MethodSymbol);
            TypeArguments = paramsCandidate.MethodSymbol.TypeArguments.Select(e => e.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)).ToList();
            TypeConstraints = CreateTypeConstraints(paramsCandidate.MethodSymbol.TypeArguments);
            ArgNameSpan = $"{paramsCandidate.SpanParamName}Span";
            ArgNameSpanInput = Candidate.IsSpanRefType
                ? $"ref {ArgNameSpan}"
                : ArgNameSpan;
        }
        
        public SuccessfulParamsCandidate Candidate { get; }

        public string ReturnType { get; }

        public string SpanArgumentType { get; }

        public List<ParameterInfo> ParameterInfos { get; }
        
        public List<string> FixArguments { get; }
        
        public ReturnKind ReturnsKind { get; }

        public List<string> TypeArguments { get; }

        public List<TypeConstrainInfo> TypeConstraints { get; }

        public string ArgName => Candidate.SpanParamName;

        public string ArgNameSpan { get; }
        
        public string ArgNameSpanInput { get; }

        public string MethodName => Candidate.MethodSymbol.Name;

        public bool IsStatic => Candidate.MethodSymbol.IsStatic;

        private static string CreateReturnTypeFor(IMethodSymbol methodSymbol)
        {
            var returnType = methodSymbol.ReturnType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var isNullable = methodSymbol.ReturnType.NullableAnnotation == NullableAnnotation.Annotated;
            if (methodSymbol.ReturnsByRef)
            {
                return SemanticHelpers.WithModifiers(returnType, RefKind.Ref, isNullable);
            }
            else if (methodSymbol.ReturnsByRefReadonly)
            {
                return SemanticHelpers.WithModifiers(returnType, RefKind.RefReadOnlyParameter, isNullable);
            }

            return SemanticHelpers.WithModifiers(returnType, RefKind.None, isNullable);
        }

        private static string GetSpanArgumentType(IParameterSymbol spanParam)
        {
            var spanType = spanParam.Type as INamedTypeSymbol;
            SemanticHelpers.AssertNotNull(spanType);
            var spanTypeArgument = spanType.TypeArguments.First();
            string spanTypeName = spanTypeArgument.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            bool isNullable = spanTypeArgument.NullableAnnotation == NullableAnnotation.Annotated;
            return SemanticHelpers.WithModifiers(spanTypeName, RefKind.None, isNullable);
        }

        private static List<ParameterInfo> GetNonParamsArguments(IMethodSymbol methodSymbol)
        {
            return methodSymbol.Parameters
                .Take(methodSymbol.Parameters.Length - 1)
                .Select(arg => new ParameterInfo(arg))
                .ToList();
        }

        private static List<TypeConstrainInfo> CreateTypeConstraints(ImmutableArray<ITypeSymbol> typeArguments)
        {
            var typeConstraintsList = new List<TypeConstrainInfo>();
            foreach (var typeArg in typeArguments.Cast<ITypeParameterSymbol>())
            {
                var typeConstraints = new List<string>();
                if (typeArg.HasUnmanagedTypeConstraint)
                {
                    typeConstraints.Add("unmanaged");
                }
                else if (typeArg.HasValueTypeConstraint)
                {
                    typeConstraints.Add("struct");
                }
                else if (typeArg.HasReferenceTypeConstraint)
                {
                    typeConstraints.Add("class");
                }
                else if (typeArg.HasNotNullConstraint)
                {
                    typeConstraints.Add("notnull");
                }
                if (typeArg.ConstraintTypes.Length > 0)
                {
                    foreach (var item in typeArg.ConstraintTypes)
                    {
                        typeConstraints.Add(item.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                    }
                }
                if (typeArg.HasConstructorConstraint)
                {
                    typeConstraints.Add("new()");
                }
                if (typeConstraints.Count > 0)
                {
                    typeConstraintsList.Add(new TypeConstrainInfo
                    {
                        Type = typeArg.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                        Constraints = typeConstraints
                    });
                }
            }
            return typeConstraintsList;
        }
    }
}

