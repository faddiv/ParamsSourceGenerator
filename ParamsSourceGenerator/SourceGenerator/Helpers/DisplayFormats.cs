using Microsoft.CodeAnalysis;

namespace Foxy.Params.SourceGenerator.Helpers;

internal class DisplayFormats
{
    public static SymbolDisplayFormat ForFileName => new(
        globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        genericsOptions: SymbolDisplayGenericsOptions.None);
}