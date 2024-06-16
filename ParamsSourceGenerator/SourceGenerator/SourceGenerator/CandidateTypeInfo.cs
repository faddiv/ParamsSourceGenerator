using System;
using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.SourceGenerator
{
    public class CandidateTypeInfo : IEquatable<CandidateTypeInfo?>
    {
        public required string TypeName { get; init; }

        public required bool InGlobalNamespace { get; init; }

        public required string[] TypeHierarchy { get; init; }

        public required string Namespace { get; init; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as CandidateTypeInfo);
        }

        public bool Equals(CandidateTypeInfo? other)
        {
            return other is not null &&
                   TypeName == other.TypeName;
        }

        public override int GetHashCode()
        {
            return -448171650 + EqualityComparer<string>.Default.GetHashCode(TypeName);
        }
    }
}