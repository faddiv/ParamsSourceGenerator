// <auto-generated />

#nullable enable

namespace Something
{
    partial class Foo
    {
        public static global::Something.Foo.InnerClass Format(global::Something.Foo.InnerClass format, global::Something.Foo.InnerClass args0)
        {
            var args = new Arguments1<global::Something.Foo.InnerClass>(args0);
            var argsSpan = global::System.Runtime.InteropServices.MemoryMarshal.CreateReadOnlySpan(ref args.arg0, 1);
            return Format(format, argsSpan);
        }

        public static global::Something.Foo.InnerClass Format(global::Something.Foo.InnerClass format, params global::Something.Foo.InnerClass[] args)
        {
            var argsSpan = new global::System.ReadOnlySpan<global::Something.Foo.InnerClass>(args);
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