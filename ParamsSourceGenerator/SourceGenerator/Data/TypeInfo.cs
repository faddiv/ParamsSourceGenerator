using Foxy.Params.SourceGenerator.Helpers;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foxy.Params.SourceGenerator.Data;

public class TypeInfo(string typeName, string[]? genericParameters = null) : IEquatable<TypeInfo?>
{
    public string TypeName { get; } = typeName;
    public string[] GenericParameters { get; } = genericParameters ?? [];

    internal static TypeInfo Create(ITypeSymbol type)
    {
        var name = type.ToDisplayString(DisplayFormats.ForRootTypeDisplay);
        if (type is INamedTypeSymbol namedType && namedType.TypeArguments.Length > 0)
        {
            return new TypeInfo(name, namedType.TypeArguments.Select(e => e.ToDisplayString(DisplayFormats.ForGenericArgumentFormat)).ToArray());
        }
        else
        {
            return new TypeInfo(name, Array.Empty<string>());
        }
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as TypeInfo);
    }

    public bool Equals(TypeInfo? other)
    {
        return other is not null &&
               TypeName == other.TypeName &&
               CollectionComparer.GetFor(GenericParameters).Equals(GenericParameters, other.GenericParameters);
    }

    public override int GetHashCode()
    {
        int hashCode = 1224272728;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TypeName);
        hashCode = hashCode * -1521134295 + CollectionComparer.GetFor(GenericParameters).GetHashCode(GenericParameters);
        return hashCode;
    }

    public override string ToString()
    {
        return GenericParameters.Length > 0
            ? $"{TypeName}<{string.Join(", ", GenericParameters)}>"
            : TypeName;
    }
}
