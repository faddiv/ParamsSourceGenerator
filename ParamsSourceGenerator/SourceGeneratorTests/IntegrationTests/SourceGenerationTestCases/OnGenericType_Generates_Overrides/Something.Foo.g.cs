// <auto-generated />

#nullable enable

namespace Something
{
    partial class Foo<T1, T2>
    {
        public static T2 Format(T1 format, T2 args0)
        {
            var args = new Arguments1<T2>(args0);
            var argsSpan = global::System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpan(ref args.arg0, 1);
            return Format(format, argsSpan);
        }

        public static T2 Format(T1 format, params T2[] args)
        {
            var argsSpan = new global::System.ReadOnlySpan<T2>(args);
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
}