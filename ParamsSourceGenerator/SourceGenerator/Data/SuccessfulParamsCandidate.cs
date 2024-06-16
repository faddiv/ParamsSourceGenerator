using Foxy.Params.SourceGenerator.SourceGenerator;
using Microsoft.CodeAnalysis;
using System;

namespace Foxy.Params.SourceGenerator.Data;

internal class SuccessfulParamsCandidate : ParamsCandidate, IEquatable<SuccessfulParamsCandidate?>
{
    public override bool HasErrors => false;

    public required IMethodSymbol MethodSymbol { get; init; }

    public required CandidateTypeInfo TypeInfo { get; init; }
    public required INamedTypeSymbol ContainingType { get; init; }

    public required IParameterSymbol SpanParam { get; init; }

    public required int MaxOverrides { get; init; }

    public required bool HasParams { get; init; }
    
    public bool IsSpanRefType => 
        SpanParam is not null && (
        SpanParam.RefKind == RefKind.Ref ||
        SpanParam.RefKind == RefKind.RefReadOnlyParameter);

    public string SpanParamName => SpanParam?.Name ?? "";

    public override bool Equals(object? obj)
    {
        return Equals(obj as SuccessfulParamsCandidate);
    }

    public bool Equals(SuccessfulParamsCandidate? other)
    {
        return other is not null &&
               MaxOverrides == other.MaxOverrides &&
               HasParams == other.HasParams &&
               SymbolEqualityComparer.Default.Equals(MethodSymbol, other.MethodSymbol);
    }

    public override int GetHashCode()
    {
        int hashCode = 274651747;
        hashCode = hashCode * -1521134295 + MaxOverrides.GetHashCode();
        hashCode = hashCode * -1521134295 + HasParams.GetHashCode();
        hashCode = hashCode * -1521134295 + SymbolEqualityComparer.Default.GetHashCode(MethodSymbol);
        return hashCode;
    }
}

