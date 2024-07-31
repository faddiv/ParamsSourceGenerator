using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace PerformanceTest.Helpers;

[SuppressMessage("MicrosoftCodeAnalysisCompatibility", "RS1009:Only internal implementations of this interface are allowed", Justification = "<Pending>")]
public class NamedTypeSymbol : INamedTypeSymbol
{
    public int Arity => throw new NotImplementedException();

    public bool IsGenericType => throw new NotImplementedException();

    public bool IsUnboundGenericType => throw new NotImplementedException();

    public bool IsScriptClass => throw new NotImplementedException();

    public bool IsImplicitClass => throw new NotImplementedException();

    public bool IsComImport => throw new NotImplementedException();

    public bool IsFileLocal => throw new NotImplementedException();

    public IEnumerable<string> MemberNames => throw new NotImplementedException();

    public ImmutableArray<ITypeParameterSymbol> TypeParameters => throw new NotImplementedException();

    public ImmutableArray<ITypeSymbol> TypeArguments => throw new NotImplementedException();

    public ImmutableArray<NullableAnnotation> TypeArgumentNullableAnnotations => throw new NotImplementedException();

    public INamedTypeSymbol OriginalDefinition => throw new NotImplementedException();

    public IMethodSymbol? DelegateInvokeMethod => throw new NotImplementedException();

    public INamedTypeSymbol? EnumUnderlyingType => throw new NotImplementedException();

    public INamedTypeSymbol ConstructedFrom => throw new NotImplementedException();

    public ImmutableArray<IMethodSymbol> InstanceConstructors => throw new NotImplementedException();

    public ImmutableArray<IMethodSymbol> StaticConstructors => throw new NotImplementedException();

    public ImmutableArray<IMethodSymbol> Constructors => throw new NotImplementedException();

    public ISymbol? AssociatedSymbol => throw new NotImplementedException();

    public bool MightContainExtensionMethods => throw new NotImplementedException();

    public INamedTypeSymbol? TupleUnderlyingType => throw new NotImplementedException();

    public ImmutableArray<IFieldSymbol> TupleElements => throw new NotImplementedException();

    public bool IsSerializable => throw new NotImplementedException();

    public INamedTypeSymbol? NativeIntegerUnderlyingType => throw new NotImplementedException();

    public TypeKind TypeKind => throw new NotImplementedException();

    public INamedTypeSymbol? BaseType => throw new NotImplementedException();

    public ImmutableArray<INamedTypeSymbol> Interfaces => throw new NotImplementedException();

    public ImmutableArray<INamedTypeSymbol> AllInterfaces => throw new NotImplementedException();

    public bool IsReferenceType => throw new NotImplementedException();

    public bool IsValueType => throw new NotImplementedException();

    public bool IsAnonymousType => throw new NotImplementedException();

    public bool IsTupleType => throw new NotImplementedException();

    public bool IsNativeIntegerType => throw new NotImplementedException();

    public SpecialType SpecialType => throw new NotImplementedException();

    public bool IsRefLikeType => throw new NotImplementedException();

    public bool IsUnmanagedType => throw new NotImplementedException();

    public bool IsReadOnly => throw new NotImplementedException();

    public bool IsRecord => throw new NotImplementedException();

    public NullableAnnotation NullableAnnotation => throw new NotImplementedException();

    public bool IsNamespace => throw new NotImplementedException();

    public bool IsType => throw new NotImplementedException();

    public SymbolKind Kind => throw new NotImplementedException();

    public string Language => throw new NotImplementedException();

    public string Name { get; set; } = "";

    public string MetadataName => throw new NotImplementedException();

    public int MetadataToken => throw new NotImplementedException();

    public ISymbol ContainingSymbol => throw new NotImplementedException();

    public IAssemblySymbol ContainingAssembly => throw new NotImplementedException();

    public IModuleSymbol ContainingModule => throw new NotImplementedException();

    public INamedTypeSymbol? ContainingType { get; set; }

    public INamespaceSymbol ContainingNamespace => throw new NotImplementedException();

    public bool IsDefinition => throw new NotImplementedException();

    public bool IsStatic => throw new NotImplementedException();

    public bool IsVirtual => throw new NotImplementedException();

    public bool IsOverride => throw new NotImplementedException();

    public bool IsAbstract => throw new NotImplementedException();

    public bool IsSealed => throw new NotImplementedException();

    public bool IsExtern => throw new NotImplementedException();

    public bool IsImplicitlyDeclared => throw new NotImplementedException();

    public bool CanBeReferencedByName => throw new NotImplementedException();

    public ImmutableArray<Location> Locations => throw new NotImplementedException();

    public ImmutableArray<SyntaxReference> DeclaringSyntaxReferences => throw new NotImplementedException();

    public Accessibility DeclaredAccessibility => throw new NotImplementedException();

    public bool HasUnsupportedMetadata => throw new NotImplementedException();

    ITypeSymbol ITypeSymbol.OriginalDefinition => throw new NotImplementedException();

    ISymbol ISymbol.OriginalDefinition => throw new NotImplementedException();

    public void Accept(SymbolVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public TResult? Accept<TResult>(SymbolVisitor<TResult> visitor)
    {
        throw new NotImplementedException();
    }

    public TResult Accept<TArgument, TResult>(SymbolVisitor<TArgument, TResult> visitor, TArgument argument)
    {
        throw new NotImplementedException();
    }

    public INamedTypeSymbol Construct(params ITypeSymbol[] typeArguments)
    {
        throw new NotImplementedException();
    }

    public INamedTypeSymbol Construct(ImmutableArray<ITypeSymbol> typeArguments, ImmutableArray<NullableAnnotation> typeArgumentNullableAnnotations)
    {
        throw new NotImplementedException();
    }

    public INamedTypeSymbol ConstructUnboundGenericType()
    {
        throw new NotImplementedException();
    }

    public bool Equals(ISymbol? other, SymbolEqualityComparer equalityComparer)
    {
        throw new NotImplementedException();
    }

    public bool Equals(ISymbol? other)
    {
        throw new NotImplementedException();
    }

    public ISymbol? FindImplementationForInterfaceMember(ISymbol interfaceMember)
    {
        throw new NotImplementedException();
    }

    public ImmutableArray<AttributeData> GetAttributes()
    {
        throw new NotImplementedException();
    }

    public string? GetDocumentationCommentId()
    {
        throw new NotImplementedException();
    }

    public string? GetDocumentationCommentXml(CultureInfo? preferredCulture = null, bool expandIncludes = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ImmutableArray<ISymbol> GetMembers()
    {
        throw new NotImplementedException();
    }

    public ImmutableArray<ISymbol> GetMembers(string name)
    {
        throw new NotImplementedException();
    }

    public ImmutableArray<CustomModifier> GetTypeArgumentCustomModifiers(int ordinal)
    {
        throw new NotImplementedException();
    }

    public ImmutableArray<INamedTypeSymbol> GetTypeMembers()
    {
        throw new NotImplementedException();
    }

    public ImmutableArray<INamedTypeSymbol> GetTypeMembers(string name)
    {
        throw new NotImplementedException();
    }

    public ImmutableArray<INamedTypeSymbol> GetTypeMembers(string name, int arity)
    {
        throw new NotImplementedException();
    }

    public ImmutableArray<SymbolDisplayPart> ToDisplayParts(NullableFlowState topLevelNullability, SymbolDisplayFormat? format = null)
    {
        throw new NotImplementedException();
    }

    public ImmutableArray<SymbolDisplayPart> ToDisplayParts(SymbolDisplayFormat? format = null)
    {
        throw new NotImplementedException();
    }

    public string ToDisplayString(NullableFlowState topLevelNullability, SymbolDisplayFormat? format = null)
    {
        throw new NotImplementedException();
    }

    public string ToDisplayString(SymbolDisplayFormat? format = null)
    {
        return Name;
    }

    public ImmutableArray<SymbolDisplayPart> ToMinimalDisplayParts(SemanticModel semanticModel, NullableFlowState topLevelNullability, int position, SymbolDisplayFormat? format = null)
    {
        throw new NotImplementedException();
    }

    public ImmutableArray<SymbolDisplayPart> ToMinimalDisplayParts(SemanticModel semanticModel, int position, SymbolDisplayFormat? format = null)
    {
        throw new NotImplementedException();
    }

    public string ToMinimalDisplayString(SemanticModel semanticModel, NullableFlowState topLevelNullability, int position, SymbolDisplayFormat? format = null)
    {
        throw new NotImplementedException();
    }

    public string ToMinimalDisplayString(SemanticModel semanticModel, int position, SymbolDisplayFormat? format = null)
    {
        throw new NotImplementedException();
    }

    public ITypeSymbol WithNullableAnnotation(NullableAnnotation nullableAnnotation)
    {
        throw new NotImplementedException();
    }
}