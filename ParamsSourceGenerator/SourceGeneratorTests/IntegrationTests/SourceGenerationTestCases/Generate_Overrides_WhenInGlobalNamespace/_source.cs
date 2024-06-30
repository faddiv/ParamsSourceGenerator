using Foxy.Params;
using System;

public partial class Foo
{
    [Params(MaxOverrides = 1)]
    private static void Format(string format, ReadOnlySpan<object> args)
    {
    }
}
