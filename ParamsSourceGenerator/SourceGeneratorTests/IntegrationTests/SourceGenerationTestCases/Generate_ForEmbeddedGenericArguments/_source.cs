using Foxy.Params;
using System;
using static Something.Foo;

namespace Something;

public partial class Foo
{
    public class OuterClass<T>
    {
    }

    public class InnerClass<T>
    {
    }

    [Params(MaxOverrides = 1)]
    private static OuterClass<InnerClass<object>> Format(OuterClass<InnerClass<object>> format, ReadOnlySpan<OuterClass<InnerClass<object>>> args)
    {
        return format;
    }
}
