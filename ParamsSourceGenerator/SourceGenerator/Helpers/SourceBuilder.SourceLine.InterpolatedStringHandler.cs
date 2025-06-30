using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Foxy.Params.SourceGenerator.Helpers;

internal partial class SourceBuilder
{
    public readonly ref partial struct SourceLine
    {
        [InterpolatedStringHandler]
        public readonly ref struct SourceLineInterpolatedStringHandler
        {
            private readonly SourceLine _sourceLine;

            public SourceLineInterpolatedStringHandler(int literalLength, int formattedCount, in SourceLine sourceLine)
            {
                _sourceLine = sourceLine;
                _sourceLine._builder
                    .EnsureCapacity(_sourceLine._builder._builder.Length + literalLength + (formattedCount << 4));
            }


            public readonly void AppendLiteral(string s)
            {
                _sourceLine.AddSegment(s);
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
                    _sourceLine.AppendFormatted(t);
                }
            }

            public readonly void AppendFormatted(string? arg)
            {
                if (arg is not null)
                {
                    _sourceLine.AddSegment(arg);
                }
            }

            public readonly void AppendFormatted(int arg)
            {
                _sourceLine.Append(arg);
            }

            public readonly void AppendFormatted(int? arg)
            {
                if (arg.HasValue)
                {
                    _sourceLine.Append(arg.Value);
                }
            }

            public readonly void AppendFormatted(IEnumerable<string> args)
            {
                _sourceLine.AddCommaSeparatedList(args);
            }
        }
    }
}
