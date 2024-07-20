using System.Text;

namespace Foxy.Params.SourceGenerator.Rendering;

internal class FileNameOutput : IRenderOutput
{
    private readonly StringBuilder _output;
    public FileNameOutput()
    {
        _output = new StringBuilder(128);
    }

    public void Append(string text)
    {
        _output.Append(text);
    }
    
    public void SeparatorDot()
    {
        _output.Append('.');
    }

    public void AddExtension()
    {
        _output.Append(".g.cs");
    }

    public override string ToString()
    {
        return _output.ToString();
    }
}