using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
