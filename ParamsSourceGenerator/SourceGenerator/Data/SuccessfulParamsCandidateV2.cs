using Foxy.Params.SourceGenerator.NewData;

namespace Foxy.Params.SourceGenerator.Data;

internal class SuccessfulParamsCandidateV2 : SuccessfulParamsV2
{
    public ClassTypeElement TypeInfo => MethodInfo.Parent;
}

