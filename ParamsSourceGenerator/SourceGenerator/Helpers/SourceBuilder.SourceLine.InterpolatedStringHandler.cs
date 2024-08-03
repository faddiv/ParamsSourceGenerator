using Foxy.Params.SourceGenerator.Data;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Foxy.Params.SourceGenerator.Helpers;

internal partial class SourceBuilder
{
    public readonly ref partial struct SourceLine
    {
        [InterpolatedStringHandler]
        public readonly ref struct InterpolatedStringHandler
        {
            private readonly SourceLine _builder;

            public InterpolatedStringHandler(int literalLength, int formattedCount, in SourceLine builder)
            {
                _builder = builder;
            }


            public readonly void AppendLiteral(string s)
            {
                _builder.AddSegment(s);
            }

            public readonly void AppendFormatted<T>(T? t)
            {
                if (t is null)
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
                    _builder.AddSegment(arg);
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
        }
    }
}
