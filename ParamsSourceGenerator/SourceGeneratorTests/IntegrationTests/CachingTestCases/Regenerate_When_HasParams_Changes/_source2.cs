using Foxy.Params;
using System;

namespace Something;

public partial class Foo
{
    [Params(MaxOverrides = 1, HasParams = false)]
    private static T Format<T>(string format, ReadOnlySpan<object> args)
        where T : new()
    {
        return default(T);
    }
}
