// <auto-generated />

#nullable enable

namespace Something
{
    partial class Foo
    {
        public global::Something.Foo Format(string format, string args0)
        {
            var args = new Arguments1<string>(args0);
            var argsSpan = global::System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpan(ref args.arg0, 1);
            return Format(format, argsSpan);
        }

        public global::Something.Foo Format(string format, string args0, string args1)
        {
            var args = new Arguments2<string>(args0, args1);
            var argsSpan = global::System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpan(ref args.arg0, 2);
            return Format(format, argsSpan);
        }

        public global::Something.Foo Format(string format, params string[] args)
        {
            var argsSpan = new global::System.ReadOnlySpan<string>(args);
            return Format(format, argsSpan);
        }
    }

    [global::System.Runtime.CompilerServices.InlineArray(1)]
    file struct Arguments1<T>
    {
        public T arg0;

        public Arguments1(T value0)
        {
            arg0 = value0;
        }
    }

    [global::System.Runtime.CompilerServices.InlineArray(2)]
    file struct Arguments2<T>
    {
        public T arg0;

        public Arguments2(T value0, T value1)
        {
            arg0 = value0;
            this[1] = value1;
        }
    }
}
