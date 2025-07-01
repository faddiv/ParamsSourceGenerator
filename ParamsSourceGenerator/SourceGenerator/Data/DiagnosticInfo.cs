using Foxy.Params.SourceGenerator.Helpers;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foxy.Params.SourceGenerator.Data;

internal class DiagnosticInfo(DiagnosticDescriptor descriptor, Location location, params object[] args) 
    : IEquatable<DiagnosticInfo?>
{
    public DiagnosticDescriptor Descriptor { get; } = descriptor;
    
    public Location Location { get; } = location;
    
    public object[] Args { get; } = args;

    internal static DiagnosticInfo Create(DiagnosticDescriptor descriptor, Location location, params object[] args)
    {
        return new DiagnosticInfo(descriptor, location, args);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as DiagnosticInfo);
    }

    public bool Equals(DiagnosticInfo? other)
    {
        return other is not null &&
               EqualityComparer<DiagnosticDescriptor>.Default.Equals(Descriptor, other.Descriptor) &&
               Args.SequenceEqual(other.Args);
    }

    public override int GetHashCode()
    {
        int hashCode = 2011230944;
        hashCode = hashCode * -1521134295 + EqualityComparer<DiagnosticDescriptor>.Default.GetHashCode(Descriptor);
        hashCode = hashCode * -1521134295 + CollectionComparer.GetHashCode(Args);
        return hashCode;
    }

    internal Diagnostic ToDiagnostics()
    {
        return Diagnostic.Create(Descriptor, Location, Args);
    }
}
