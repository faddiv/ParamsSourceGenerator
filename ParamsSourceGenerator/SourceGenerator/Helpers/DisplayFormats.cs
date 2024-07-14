using Microsoft.CodeAnalysis;

namespace Foxy.Params.SourceGenerator.Helpers;

internal class DisplayFormats
{
    public static SymbolDisplayFormat ForFileName => new(
        globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        genericsOptions: SymbolDisplayGenericsOptions.None);

    public static SymbolDisplayFormat ForRootTypeDisplay =
        SymbolDisplayFormat.FullyQualifiedFormat
        .WithGenericsOptions(SymbolDisplayGenericsOptions.None)
        .AddMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier);

    public static SymbolDisplayFormat ForGenericArgumentFormat =
        SymbolDisplayFormat.FullyQualifiedFormat
        .AddMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier);

}