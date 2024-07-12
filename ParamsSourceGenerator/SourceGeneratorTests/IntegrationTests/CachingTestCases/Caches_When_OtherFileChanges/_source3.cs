using Foxy.Params;
using System;

namespace Something;

public partial class Baz
{
    [Params(MaxOverrides = 1)]
    private static T Format<T>(int format, ReadOnlySpan<object> args)
        where T : new()
    {
        return new T();
    }
}
