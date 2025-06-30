using System;
using System.Runtime.CompilerServices;

namespace Foxy.Params.SourceGenerator.Helpers;

internal partial class SourceBuilder
{
    public Block StartBlock()
    {
        OpenBlock();
        return new Block(this, true);
    }

    public Block StartIndented()
    {
        IncreaseIntend();
        return new Block(this, false);
    }

    private void OpenBlock()
    {
        AddLineInternal("{");
        IncreaseIntend();
    }

    public void CloseBlock()
    {
        DecreaseIntend();
        AddLineInternal("}");
    }

    public readonly struct Block(SourceBuilder builder, bool addClosingBracket) : IDisposable
    {
        private readonly bool _addClosingBracket = addClosingBracket;

        public void Dispose()
        {
            if (_addClosingBracket)
            {
                builder.CloseBlock();
            }
            else
            {
                builder.DecreaseIntend();
            }
        }
    }
}
