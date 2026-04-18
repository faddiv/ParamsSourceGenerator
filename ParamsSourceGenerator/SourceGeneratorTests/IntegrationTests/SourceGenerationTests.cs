using System.Threading.Tasks;
using Foxy.Params.SourceGenerator;
using Xunit;
using SourceGeneratorTests.TestInfrastructure;
using SourceGeneratorTests.TestInfrastructure.Verifiers;
using Test.Infrastructure;

namespace SourceGeneratorTests.IntegrationTests;

using VerifyCS = CSharpSourceGeneratorVerifier<ParamsIncrementalGenerator>;

public class SourceGenerationTests(TestEnvironment testEnvironment)
{
    [Fact]
    public async Task Always_Generate_ParamsAttribute()
    {
        var code = new CSharpFile("Empty.cs", "");

        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.DefaultOutput);
    }

    [Fact]
    public async Task Generate_OverridesFor_ReadOnlySpan_WithDefaultParameters()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }


    [Fact]
    public async Task Generate_OverridesFor_CountedCase_WithMaxOverrides()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_OverridesFor_MultipleFixedParameters()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task DoesNotGenerateParams_WhenHasParamsIsFalse()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForInstanceLevelMethod()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForNonObjectReadOnlySpan()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForFunctions_WithKeywordReturnType()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForFunctions_WithNonKeywordReturnType()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForGenericMethods_WithMultipleGenericFixedParameters()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForGenericMethods_WithGenericParamsParameter()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForGenericFunctions_WithGenericReturnType()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForGenericMethods_WithRestrictedGenericParameters()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForGenericMethods_WithRestrictedGenericParamsParameters()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForGenericMethods_WithRestrictedGenericReturnType()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForCustomTypeParametersAndReturnType()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForRefReadOnlySpan()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForRefReadonlyReadOnlySpan()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForInReadOnlySpan()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForSpecialFixedParams()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForRefReturn()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForRefReadonlyReturn()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForNullableParametersAndReturnType()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForDifferentReadOnlySpanName()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_WhenParametersDontCollide()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_Overrides_WhenClassIsEmbedded()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_Overrides_WhenNamespaceIsEmbedded()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_Overrides_WhenInGlobalNamespace()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task OnGenericType_Generates_Overrides()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_ForEmbeddedGenericArguments()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }

    [Fact]
    public async Task Generate_OverridesFor_AliasedParam()
    {
        var code = testEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            testEnvironment.GetOutputs());
    }
}
