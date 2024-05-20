// <auto-generated />

#nullable enable

namespace Something
{
    partial class Foo
    {
        public static void Format(global::System.Exception ex, string format, object args0)
        {
            var args = new Arguments1<object>(args0);
            var argsSpan = global::System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpan(ref args.arg0, 1);
            Format(ex, format, argsSpan);
        }

        public static void Format(global::System.Exception ex, string format, object args0, object args1)
        {
            var args = new Arguments2<object>(args0, args1);
            var argsSpan = global::System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpan(ref args.arg0, 2);
            Format(ex, format, argsSpan);
        }

        public static void Format(global::System.Exception ex, string format, params object[] args)
        {
            var argsSpan = new global::System.ReadOnlySpan<object>(args);
            Format(ex, format, argsSpan);
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
