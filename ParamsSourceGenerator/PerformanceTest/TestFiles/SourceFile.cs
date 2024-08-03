using Foxy.Params;

namespace Foo.Bar.Baz
{
    public partial class Alfa
    {
        public partial class Beta
        {
            public partial class Gamma
            {
                [Params(MaxOverrides = 3, HasParams = true)]
                private string Format<T, F, G, H>(string format, ReadOnlySpan<object> args)
                        where T : struct
                        where F : class, ICloneable, new()
                        where G : notnull, Attribute
                        where H : unmanaged
                {
                    return "";
                }
            }
        }
    }
}
