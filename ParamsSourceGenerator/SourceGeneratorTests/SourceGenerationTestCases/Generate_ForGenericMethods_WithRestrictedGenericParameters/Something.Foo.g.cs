// <auto-generated />

#nullable enable

namespace Something
{
    partial class Foo
    {
        public static void Format<T, F, G, H>(string format, object args0)
            where T : struct where F : class, global::System.ICloneable, new()
            where G : notnull, global::System.Attribute where H : unmanaged
        {
            var args = new Arguments1<object>(args0);
            var argsSpan = global::System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpan(ref args.arg0, 1);
            Format<T, F, G, H>(format, argsSpan);
        }

        public static void Format<T, F, G, H>(string format, params object[] args)
            where T : struct
            where F : class, global::System.ICloneable, new()
            where G : notnull, global::System.Attribute
            where H : unmanaged
        {
            var argsSpan = new global::System.ReadOnlySpan<object>(args);
            Format<T, F, G, H>(format, argsSpan);
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
