using System.Collections.Generic;

namespace Foxy.Params.SourceGenerator.Helpers;

internal partial class SourceBuilder
{
    public readonly ref struct SourceLine
    {
        private readonly SourceBuilder _builder;

        public SourceLine(SourceBuilder builder)
        {
            _builder = builder;
            _builder.AddIntend();
        }

        public void Returns()
        {
            _builder.AppendInternal("return ");
        }

        public void AddSegment(string segment)
        {
            _builder.AppendInternal(segment);
        }

        public void AddCommaSeparatedList(IEnumerable<string> elements)
        {
            _builder.AddCommaSeparatedList(elements);
        }

        public void EndLine()
        {
            _builder.AppendLineInternal(";");
        }
    }
}
