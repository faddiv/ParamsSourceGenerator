using Foxy.Params;
using System;

namespace Something;

public class ParentClass
{
    public partial class Bar
    {
        [{|#0:Params|}]
        private static void Format(string format, ReadOnlySpan<object> args)
        {
        }
    }
}
