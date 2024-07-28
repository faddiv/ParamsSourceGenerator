using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Foxy.Params.SourceGenerator.Helpers;

internal partial class SourceBuilder
{
    [InterpolatedStringHandler]
    public readonly ref struct InterpolatedStringHandler
    {
        private readonly SourceBuilder _builder;

        public InterpolatedStringHandler(int literalLength, int formattedCount, SourceBuilder builder)
        {
            _builder = builder;
            _builder.EnsureCapacity(_builder._builder.Length + literalLength + (formattedCount << 4));
            _builder.AddIntend();
        }


        public readonly void AppendLiteral(string s)
        {
            _builder.Append(s);
        }

        public readonly void AppendFormatted<T>(T? t)
        {
            if(t is null)
            {
                return;
            }

            if (t is IEnumerable<string> strings)
            {
                AppendFormatted(strings);
            }
            else
            {
                _builder.AppendFormatted(t);
            }
        }

        public readonly void AppendFormatted(string? arg)
        {
            if (arg is not null)
            {
                _builder.Append(arg);
            }
        }

        public readonly void AppendFormatted(int arg)
        {
            _builder.Append(arg);
        }

        public readonly void AppendFormatted(int? arg)
        {
            if (arg.HasValue)
            {
                _builder.Append(arg.Value);
            }
        }

        public readonly void AppendFormatted(IEnumerable<string> args)
        {
            _builder.AddCommaSeparatedList(args);
        }

        internal void FinishLine()
        {
            _builder.AppendLine();
        }
    }
}
