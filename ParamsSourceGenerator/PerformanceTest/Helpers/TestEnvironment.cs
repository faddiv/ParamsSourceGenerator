using Microsoft.CodeAnalysis;
using Test.Infrastructure;

namespace PerformanceTest.Helpers;

internal static class TestEnvironment
{
    private static readonly EnvironmentProvider _environment = new();
    private static readonly string _subDirectory = "TestFiles";

    public static CSharpFile GetParamsAttribute()
        => _environment.GetFile(_subDirectory, "ParamsAttribute.cs");

    public static CSharpFile GetNestedSourceFile()
        => _environment.GetFile(_subDirectory, "NestedSourceFile.cs");

    public static INamedTypeSymbol FindGamma(IAssemblySymbol symbol, string typeName)
    {
        INamedTypeSymbol? type = FindTypeByName(symbol.GlobalNamespace, typeName);
        if (type is null)
        {
            foreach (var module in symbol.Modules)
            {
                type = FindTypeByName(module.GlobalNamespace, typeName);
                if (type is not null)
                {
                    break;
                }
            }

        }
        return type ?? throw new ApplicationException($"{typeName} not found.");
    }

    public static IMethodSymbol FindMethodByName(IAssemblySymbol symbol, string typeName, string methodName)
    {
        var type = FindGamma(symbol, typeName);
        return (IMethodSymbol?)type.GetMembers(methodName).FirstOrDefault()
            ?? throw new ApplicationException($"{methodName} not found.");
    }

    private static INamedTypeSymbol? FindTypeByName(INamespaceOrTypeSymbol symbol, string typeName)
    {
        foreach (var typeSymbol in symbol.GetTypeMembers())
        {
            if(typeSymbol.Name == typeName)
            {
                return typeSymbol;
            }
            var result = FindTypeByName(typeSymbol, "Gamma");
            if(result is not null)
            {
                return result;
            }
        }
        if(symbol is INamespaceSymbol namespaceSymbol )
        {
            foreach (var namespaces in namespaceSymbol.GetNamespaceMembers())
            {
                var result = FindTypeByName(namespaces, "Gamma");
                if (result is not null)
                {
                    return result;
                }
            }
        }
        return null;
    }
}
