using System;
using System.Text;
using Foxy.Params;

namespace ConsoleApp
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Types in this assembly:");
            foreach (Type t in typeof(Program).Assembly.GetTypes())
            {
                Console.WriteLine(t.FullName);
            }
        }

        //[Params(MaxOverrides = 10)]
        public static string Format(IFormatProvider provider, string format, ReadOnlySpan<object> span)
        {
            var compositeFormat = CompositeFormat.Parse(format);
            return string.Format(provider, compositeFormat, span);
        }
    }
}
