using System;
using System.Runtime.CompilerServices;

namespace SourceGeneratorTests.TestInfrastructure;

public class EnvironmentProvider
{
    private readonly string _projectPath;

    public EnvironmentProvider([CallerFilePath] string baseFilePath = null!)
    {
        ArgumentNullException.ThrowIfNull(baseFilePath);

        _projectPath = FindDirectoryOfFile(".csproj", baseFilePath);
    }

    public CSharpFile GetFile(string name)
    {
        var filePath = Path.Combine(_projectPath, name);
        return new CSharpFile(name, File.ReadAllText(filePath));
    }

    public CSharpFile GetFile(string subDir1, string name)
    {
        var filePath = Path.Combine(_projectPath, subDir1, name);
        return new CSharpFile(name, File.ReadAllText(filePath));
    }

    public CSharpFile GetFile(string subDir1, string subDir2, string name)
    {
        var filePath = Path.Combine(_projectPath, subDir1, subDir2, name);
        return new CSharpFile(name, File.ReadAllText(filePath));
    }

    public CSharpFile[] GetFiles(string subDir1, string subDir2, string pattern)
    {
        var sourcePath = Path.Combine(_projectPath, subDir1, subDir2);
        return Directory.EnumerateFiles(sourcePath, pattern)
            .OrderBy(x => x)
            .Select(e => new CSharpFile(Path.GetFileName(e), File.ReadAllText(e)))
            .ToArray();
    }

    public string GetBasePath(string subDirectory)
    {
        return Path.Combine(_projectPath, subDirectory);
    }

    private static string FindDirectoryOfFile(string fileExtension, string baseFilePath)
    {
        var dir = Path.GetDirectoryName(baseFilePath) ??
            throw new InvalidOperationException($"Could not get directory from {baseFilePath}");

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
