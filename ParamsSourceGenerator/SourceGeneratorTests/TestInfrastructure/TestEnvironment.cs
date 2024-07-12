using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SourceGeneratorTests.TestInfrastructure;

internal class TestEnvironment
{

    private static readonly string _validTestDataDirectory;

    private static readonly string _cachingTestDataDirectory;

    private static readonly string _invalidTestDataDirectory;

    public static readonly CSharpFile DefaultOuput;

    public static readonly string AttributeImpl;

    static TestEnvironment()
    {
        var projectDirectory = FindDirectoryOfFile(".csproj");
        var integrartionTestPath = Path.Combine(projectDirectory, "IntegrationTests");
        _validTestDataDirectory = Path.Combine(integrartionTestPath, "SourceGenerationTestCases");
        _invalidTestDataDirectory = Path.Combine(integrartionTestPath, "ErrorReportingTestCases");
        _cachingTestDataDirectory = Path.Combine(integrartionTestPath, "CachingTestCases");
        AttributeImpl = File.ReadAllText(Path.Combine(_validTestDataDirectory, "Attribute.cs"));
        DefaultOuput = new("ParamsAttribute.g.cs", AttributeImpl);
    }

    public static string GetValidSource([CallerMemberName] string caller = null)
    {
        var sourcePath = Path.Combine(_validTestDataDirectory, caller, "_source.cs");
        return File.ReadAllText(sourcePath);
    }

    public static string GetInvalidSource([CallerMemberName] string caller = null)
    {
        var sourcePath = Path.Combine(_invalidTestDataDirectory, caller, "_source.cs");
        return File.ReadAllText(sourcePath);
    }

    public static CSharpFile GetCachingSource([CallerMemberName] string caller = null)
    {
        var sourcePath = Path.Combine(_cachingTestDataDirectory, caller, "_source.cs");
        return new CSharpFile(Path.GetFileName(sourcePath), File.ReadAllText(sourcePath));
    }

    public static CSharpFile[] GetCachingSources([CallerMemberName] string caller = null)
    {
        var sourcePath = Path.Combine(_cachingTestDataDirectory, caller);
        return Directory.EnumerateFiles(sourcePath, "_source*")
            .OrderBy(x => x)
            .Select(e => new CSharpFile(Path.GetFileName(e), File.ReadAllText(e)))
            .ToArray();
    }

    public static CSharpFile[] GetOuputs([CallerMemberName] string caller = null)
    {
        return GetOuputs(_validTestDataDirectory, caller);
    }

    public static CSharpFile[] GetCachingOuputs([CallerMemberName] string caller = null)
    {
        return GetOuputs(_cachingTestDataDirectory, caller);
    }

    public static CSharpFile[] GetOuputs(string baseDirectory, [CallerMemberName] string caller = null)
    {
        var basePath = Path.Combine(baseDirectory, caller);
        var sources = new List<CSharpFile>
        {
            DefaultOuput
        };

        foreach (var filePath in Directory.GetFiles(basePath, "*.cs", SearchOption.TopDirectoryOnly))
        {
            var fileName = Path.GetFileName(filePath);
            if (fileName.StartsWith("_source"))
                continue;

            var content = File.ReadAllText(filePath);
            sources.Add(new CSharpFile(fileName, content));
        }

        return [.. sources];
    }

    private static string FindDirectoryOfFile(string fileExtension, [CallerFilePath] string baseFilePath = null)
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
}
