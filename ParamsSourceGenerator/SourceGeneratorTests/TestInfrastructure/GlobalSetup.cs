using FluentAssertions;

namespace SourceGeneratorTests.TestInfrastructure;

internal class GlobalSetup
{
    static GlobalSetup()
    {
        AssertionOptions.FormattingOptions.MaxDepth = 10;
    }

    public static void Run()
    {

    }
}
