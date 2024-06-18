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
                   TypeName == other.TypeName &&
                   InGlobalNamespace == other.InGlobalNamespace &&
                   Namespace == other.Namespace;
        }

        public override int GetHashCode()
        {
            int hashCode = 1623501290;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TypeName);
            hashCode = hashCode * -1521134295 + InGlobalNamespace.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Namespace);
            return hashCode;
        }
    }
}