using Foxy.Params.SourceGenerator.Data;
using Foxy.Params.SourceGenerator.NewData;

namespace Foxy.Params.SourceGenerator.Rendering;

internal class FileNameRenderer : RendererBase<FileNameOutput>
{
    public static readonly FileNameRenderer Instance = new();

    public override void Render(SuccessfulParamsGroupCandidateV2 element, FileNameOutput output)
    {
        element.TypeInfo.ExecuteRenderer(this, output);
        output.AddExtension();
    }

    public override void Render(ClassTypeElement element, FileNameOutput output)
    {
        element.Parent.ExecuteRenderer(this, output);
        output.SeparatorDot();
        output.Append(element.Name);
    }

    public override void Render(NamespaceElement element, FileNameOutput output)
    {
        if (element.Parent is not GlobalNamespaceElement)
        {
            element.Parent.ExecuteRenderer(this, output);
            output.SeparatorDot();
        }
        output.Append(element.Name);
    }
}