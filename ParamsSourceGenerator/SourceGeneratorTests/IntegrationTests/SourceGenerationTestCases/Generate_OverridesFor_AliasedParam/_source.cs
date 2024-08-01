using Foxy.Params;
using System;
using Baz = Foxy.Params.ParamsAttribute;

namespace Something;

public partial class Foo
{
    [Baz]
    private static void Format(string format, ReadOnlySpan<object> args)
    {
    }
}
