using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Foxy.Params.SourceGenerator.Helpers;

internal partial class SourceBuilder
{
    public readonly ref partial struct SourceLine : IDisposable
    {
        private readonly SourceBuilder _builder;

        public SourceLine(SourceBuilder builder)
        {
            _builder = builder;
            _builder.AddIntend();
        }

        public void Dispose()
        {
            _builder.AppendLine();
        }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Style",
            "IDE0060:Remove unused parameter",
            Justification = "Used by InterpolatedStringHandler")]
        internal void AddFormatted(
            [InterpolatedStringHandlerArgument("")] in SourceLineInterpolatedStringHandler input)
        {
        }

        public readonly void AddSegment(string segment)
        {
            _builder.AppendInternal(segment);
        }

        public readonly void AddCommaSeparatedList(IEnumerable<string> elements)
        {
            _builder.AddCommaSeparatedList(elements);
        }

        public CommaSeparatedWriter StartCommaSeparatedList()
        {
            return new CommaSeparatedWriter(_builder);
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

    internal ref struct CommaSeparatedWriter(SourceBuilder builder)
    {
        private readonly SourceBuilder _builder = builder;
        private bool _addSeparator = false;

        public void AddElement(string element)
        {
            if (_addSeparator)
            {
                _builder.AppendInternal(", ");
            } else
            {
                _addSeparator = true;
            }
            _builder.Append(element);
        }
    }
}

