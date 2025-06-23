using System;
using System.Text;
using Foxy.Params;
// ReSharper disable UnusedParameter.Local

namespace ConsoleApp
{
    partial class Program
    {
        static void Main()
        {
            Console.WriteLine("Types in this assembly:");
            foreach (Type t in typeof(Program).Assembly.GetTypes())
            {
                Console.WriteLine(t.FullName);
            }
        }

        [Params(MaxOverrides = 10)]
        public static string Format(IFormatProvider provider, string format, ReadOnlySpan<object> span)
        {
            var compositeFormat = CompositeFormat.Parse(format);
            return string.Format(provider, compositeFormat, span);
        }
    }

    public partial class Foo<T>
        where T : class
    {
        [Params(MaxOverrides = 1)]
        private static T Format(string format, ReadOnlySpan<T> args)
        {
            return default(T);
        }
    }
}
