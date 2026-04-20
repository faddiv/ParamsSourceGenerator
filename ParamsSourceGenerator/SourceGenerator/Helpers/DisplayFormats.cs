using Microsoft.CodeAnalysis;

namespace Foxy.Params.SourceGenerator.Helpers;

internal class DisplayFormats
{
    public static readonly SymbolDisplayFormat ForFileName = new(
        globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        genericsOptions: SymbolDisplayGenericsOptions.None);

    public static readonly SymbolDisplayFormat ForRootTypeDisplay =
        SymbolDisplayFormat.FullyQualifiedFormat
        .WithGenericsOptions(SymbolDisplayGenericsOptions.None)
        .AddMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier);

    public static readonly SymbolDisplayFormat ForGenericArgumentFormat =
        SymbolDisplayFormat.FullyQualifiedFormat
        .AddMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier);
}
