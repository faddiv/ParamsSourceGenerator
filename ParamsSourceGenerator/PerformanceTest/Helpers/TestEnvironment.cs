using Microsoft.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace SourceGeneratorTests.TestInfrastructure;

internal static class TestEnvironment
{
    private static string _testFilesPath;

    static TestEnvironment()
    {
        var projectDirectory = FindDirectoryOfFile(".csproj");
        _testFilesPath = Path.Combine(projectDirectory, "TestFiles");
    }

    public static CSharpFile GetFile(string name)
    {
        var filePath = Path.Combine(_testFilesPath, name);
        return new CSharpFile(name, File.ReadAllText(filePath));
    }

    private static string FindDirectoryOfFile(string fileExtension, [CallerFilePath] string baseFilePath = null!)
    {
        var dir =
            Path.GetDirectoryName(baseFilePath) ?? throw new InvalidOperationException($"Could not get directory from {baseFilePath}");

        while (Directory.GetFiles(dir, $"*{fileExtension}", SearchOption.TopDirectoryOnly).Length == 0)
        {
            dir = Path.GetDirectoryName(dir);
            if (dir == null)
            {
                throw new InvalidOperationException($"Could not find directory from file {baseFilePath}");
            }
        }

        return dir;
    }

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
