using System.Threading.Tasks;
using Foxy.Params.SourceGenerator;
using Xunit;
using SourceGeneratorTests.TestInfrastructure;

namespace SourceGeneratorTests.IntegrationTests;

using VerifyCS = CSharpSourceGeneratorVerifier<ParamsIncrementalGenerator>;

public class SourceGenerationTests
{
    [Fact]
    public async Task Always_Generate_ParamsAttribute()
    {
        var code = new CSharpFile("Empty.cs", "");

        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.DefaultOuput);
    }

    [Fact]
    public async Task Generate_OverridesFor_ReadOnlySpan_WithDefaultParameters()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }


    [Fact]
    public async Task Generate_OverridesFor_CountedCase_WithMaxOverrides()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_OverridesFor_MultipleFixedParameters()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task DoesNotGenerateParams_WhenHasParamsIsFalse()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForInstanceLevelMethod()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForNonObjectReadOnlySpan()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForFunctions_WithKeywordReturnType()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForFunctions_WithNonKeywordReturnType()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForGenericMethods_WithMultipleGenericFixedParameters()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForGenericMethods_WithGenericParamsParameter()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForGenericFunctions_WithGenericReturnType()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForGenericMethods_WithRestrictedGenericParameters()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForGenericMethods_WithRestrictedGenericParamsParameters()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForGenericMethods_WithRestrictedGenericReturnType()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForCustomTypeParametersAndReturnType()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForRefReadOnlySpan()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForRefReadonlyReadOnlySpan()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForInReadOnlySpan()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForSpecialFixedParams()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForRefReturn()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForRefReadonlyReturn()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForNullableParametersAndReturnType()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForDifferentReadOnlySpanName()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_WhenParametersDontCollide()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_Overrides_WhenClassIsEmbedded()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_Overrides_WhenNamespaceIsEmbedded()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_Overrides_WhenInGlobalNamespace()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task OnGenericType_Generates_Overrides()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_ForEmbeddedGenericArguments()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }

    [Fact]
    public async Task Generate_OverridesFor_AliasedParam()
    {
        var code = TestEnvironment.GetValidSource();
        await VerifyCS.VerifyGeneratorAsync(code,
            TestEnvironment.GetOuputs());
    }
}
