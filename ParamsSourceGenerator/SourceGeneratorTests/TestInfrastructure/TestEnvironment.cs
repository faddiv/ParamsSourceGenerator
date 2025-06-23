using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Test.Infrastructure;

namespace SourceGeneratorTests.TestInfrastructure;

internal class TestEnvironment
{
    private static readonly EnvironmentProvider _environment = new();

    private static readonly string _validTests = "IntegrationTests\\SourceGenerationTestCases";
    private static readonly string _invalidTests = "IntegrationTests\\ErrorReportingTestCases";
    private static readonly string _cachingTests = "IntegrationTests\\CachingTestCases";

    public static readonly CSharpFile DefaultOuput;

    public static CompilerRunner Compiler {get;}

    static TestEnvironment()
    {
        DefaultOuput = _environment.GetFile(_validTests, "ParamsAttribute.g.cs");
        Compiler = new CompilerRunner();
        Compiler.LoadCSharpAssemblies().GetAwaiter().GetResult();
    }

    public static CSharpFile GetValidSource([CallerMemberName] string caller = null!)
    {
        return _environment.GetFile(_validTests, caller, "_source.cs");
    }

    public static CSharpFile GetInvalidSource([CallerMemberName] string caller = null!)
    {
        return _environment.GetFile(_invalidTests, caller, "_source.cs");
    }

    public static CSharpFile GetCachingSource([CallerMemberName] string caller = null!)
    {
        return _environment.GetFile(_cachingTests, caller, "_source.cs");
    }

    public static CSharpFile[] GetCachingSources([CallerMemberName] string caller = null!)
    {
        return _environment.GetFiles(_cachingTests, caller, "_source*");
    }

    public static CSharpFile[] GetOutputs([CallerMemberName] string caller = null!)
    {
        return GetOutputsFor(_environment.GetBasePath(_validTests), caller);
    }

    public static CSharpFile[] GetCachingOutputs([CallerMemberName] string caller = null!)
    {
        return GetOutputsFor(_environment.GetBasePath(_cachingTests), caller);
    }

    public static CSharpFile[] GetOutputsFor(string baseDirectory, [CallerMemberName] string caller = null!)
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
}
