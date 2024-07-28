using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.Helpers;

internal partial class SourceBuilder
{
    public readonly ref partial struct SourceLine
    {
        private readonly SourceBuilder _builder;

        public SourceLine(SourceBuilder builder)
        {
            _builder = builder;
            _builder.AddIntend();
        }

        public readonly void AddSegment(string segment)
        {
            _builder.AppendInternal(segment);
        }

        public readonly void AddCommaSeparatedList(IEnumerable<string> elements)
        {
            _builder.AddCommaSeparatedList(elements);
        }

        public readonly void FinishLine()
        {
            _builder.AppendLine();
        }

        private void AppendFormatted<T>(T t)
        {
            if (t is not null)
            {
                _builder.Append(t.ToString());
            }
        }

        private void Append(int arg)
        {
            _builder.Append(arg);
        }
    }
}
