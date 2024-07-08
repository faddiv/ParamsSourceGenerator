using Foxy.Params;
using System;

namespace Something;

public partial class Foo<T1, T2>
    where T1 : class
    where T2 : struct
{
    [Params(MaxOverrides = 1)]
    private static T2 Format(T1 format, ReadOnlySpan<T2> args)
    {
        return default(T2);
    }
}
