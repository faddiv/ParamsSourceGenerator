using Foxy.Params.SourceGenerator.Helpers;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Foxy.Params.SourceGenerator.Data;

internal record class GeneratorData(INamedTypeSymbol TypeInfo, List<SuccessfulParamsCandidate> ParamsCandidates)
{
    public int MaxOverridesMax { get; } = ParamsCandidates.Max(e => e.MaxOverrides);
    
    public int TypeLevel { get; set; }
    
    public List<INamedTypeSymbol> TypeHierarchy { get; } = SemanticHelpers.GetTypeHierarchy(TypeInfo);
    
    public bool IsInnermostTypeLevel => TypeLevel == TypeHierarchy.Count - 1;
    
    public INamedTypeSymbol CurrentType => TypeHierarchy[TypeLevel];
    
    public DerivedData CurrentMethod { get; set; }
    
    public int ArgsCount { get; internal set; }
}