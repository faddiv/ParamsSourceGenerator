using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.Data
{
    internal class FailedParamsCandidate : ParamsCandidate, IEquatable<FailedParamsCandidate?>
    {
        public override bool HasErrors => true;

        public required List<Diagnostic> Diagnostics { get; init; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as FailedParamsCandidate);
        }

        public bool Equals(FailedParamsCandidate? other)
        {
            return other is not null &&
                   EqualityComparer<List<Diagnostic>>.Default.Equals(Diagnostics, other.Diagnostics);
        }

        public override int GetHashCode()
        {
            return 244270639 + EqualityComparer<List<Diagnostic>>.Default.GetHashCode(Diagnostics);
        }
    }
}

