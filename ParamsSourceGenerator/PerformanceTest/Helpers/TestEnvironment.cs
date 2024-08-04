using Microsoft.CodeAnalysis;

namespace SourceGeneratorTests.TestInfrastructure;

internal static class TestEnvironment
{
    private static readonly EnvironmentProvider _environment = new();
    private static readonly string _subDirectory = "TestFiles";

    public static CSharpFile GetParamsAttribute()
        => _environment.GetFile(_subDirectory, "ParamsAttribute.cs");

    public static CSharpFile GetNestedSourceFile()
        => _environment.GetFile(_subDirectory, "NestedSourceFile.cs");

    public static INamedTypeSymbol FindGamma(IAssemblySymbol symbol)
    {
        INamedTypeSymbol? type = FindInNamespaces(symbol.GlobalNamespace);
        if (type is null)
        {
            foreach (var module in symbol.Modules)
            {
                type = FindInNamespaces(module.GlobalNamespace);
                if (type is not null)
                {
                    break;
                }
            }

        }
        return type ?? throw new ApplicationException("Gamma not found.");
    }

    public static IMethodSymbol FindFormat(IAssemblySymbol symbol)
    {
        var type = FindGamma(symbol);
        return (IMethodSymbol?)type.GetMembers("Format").FirstOrDefault()
            ?? throw new ApplicationException("Format not found.");
    }

    private static INamedTypeSymbol? FindInNamespaces(INamespaceOrTypeSymbol symbol)
    {
        foreach (var typeSymbol in symbol.GetTypeMembers())
        {
            if(typeSymbol.Name == "Gamma")
            {
                return typeSymbol;
            }
            var result = FindInNamespaces(typeSymbol);
            if(result is not null)
            {
                return result;
            }
        }
        if(symbol is INamespaceSymbol namespaceSymbol )
        {
            foreach (var namespaces in namespaceSymbol.GetNamespaceMembers())
            {
                var result = FindInNamespaces(namespaces);
                if (result is not null)
                {
                    return result;
                }
            }
        }
        return null;
    }
}
