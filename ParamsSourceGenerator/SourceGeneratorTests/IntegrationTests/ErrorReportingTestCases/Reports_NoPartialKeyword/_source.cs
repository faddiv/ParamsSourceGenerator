using Foxy.Params;
using System;

namespace Something;

public class Foo
{
    [{|#0:Params|}]
    private static void Format(string format, ReadOnlySpan<object> args)
    {
    }
}
