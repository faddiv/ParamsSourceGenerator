using System;

namespace Foxy.Params.SourceGenerator.Helpers;

internal partial class SourceBuilder
{
    public Block StartBlock()
    {
        OpenBlock();
        return new Block(this);
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

    public readonly struct Block(SourceBuilder builder) : IDisposable
    {
        public void Dispose()
        {
            builder.CloseBlock();
        }
    }
}
