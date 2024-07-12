using System.Threading.Tasks;
using FluentAssertions;
using Foxy.Params.SourceGenerator.Data;
using Xunit;
using SourceGeneratorTests.TestInfrastructure;
using Foxy.Params.SourceGenerator;
using static SourceGeneratorTests.TestInfrastructure.CachingTestHelpers;

namespace SourceGeneratorTests.IntegrationTests
{
    public class CachingTests
    {

        // A collection of all the tracking names. I'll show how to simplify this later
        private static string[] _allTrackingNames = [TrackingNames.GetSpanParamsMethods, TrackingNames.NotNullFilter];

        public CachingTests()
        {
            GlobalSetup.Run();
        }

        [Fact]
        public async Task Caches_When_Nothing_Changes()
        {
            var runner = new SourceGeneratorTestRunner<ParamsIncrementalGenerator>();
            var input = TestEnvironment.GetCachingSource();
            var expected = TestEnvironment.GetCachingOuputs();

            await runner.LoadCSharpAssemblies();

            var compilation = runner.CompileSourceTexts(input);

            var result1 = runner.RunSourceGenerator(compilation);

            var compilation2 = compilation.Clone();

            var result2 = runner.RunSourceGenerator(compilation2);

            AssertRunsEqual(result1, result2, _allTrackingNames);
            AssertAllStepsCached(result2);
            result1.Diagnostics.Should().BeEmpty();
            AssertOutputs(result1, expected);
        }

        [Fact]
        public async Task Caches_When_MethodBody_Changes()
        {
            var runner = new SourceGeneratorTestRunner<ParamsIncrementalGenerator>();
            var inputs = TestEnvironment.GetCachingSources();
            var expected = TestEnvironment.GetCachingOuputs();

            await runner.LoadCSharpAssemblies();

            var compilation = runner.CompileSourceTexts(inputs[0].Content);

            var result1 = runner.RunSourceGenerator(compilation);

            var compilation2 = runner.CompileSourceTexts(inputs[1].Content);

            var result2 = runner.RunSourceGenerator(compilation2);

            AssertRunsEqual(result1, result2, _allTrackingNames);
            AssertAllStepsCached(result2);
            result1.Diagnostics.Should().BeEmpty();
            AssertOutputs(result1, expected);
        }

        [Fact]
        public async Task Regenerate_When_MaxOverrides_Changes()
        {
            var runner = new SourceGeneratorTestRunner<ParamsIncrementalGenerator>();
            var inputs = TestEnvironment.GetCachingSources();
            var expected = TestEnvironment.GetCachingOuputs();

            await runner.LoadCSharpAssemblies();

            var compilation = runner.CompileSourceTexts(inputs[0].Content);

            var result1 = runner.RunSourceGenerator(compilation);

            var compilation2 = runner.CompileSourceTexts(inputs[1].Content);

            var result2 = runner.RunSourceGenerator(compilation2);

            AssertsModifiedOnLastStep(result2);
            result2.Diagnostics.Should().BeEmpty();
            AssertOutputs(result2, expected);
        }

        [Fact]
        public async Task Regenerate_When_HasParams_Changes()
        {
            var runner = new SourceGeneratorTestRunner<ParamsIncrementalGenerator>();
            var inputs = TestEnvironment.GetCachingSources();
            var expected = TestEnvironment.GetCachingOuputs();

            await runner.LoadCSharpAssemblies();

            var compilation = runner.CompileSourceTexts(inputs[0].Content);

            var result1 = runner.RunSourceGenerator(compilation);

            var compilation2 = runner.CompileSourceTexts(inputs[1].Content);

            var result2 = runner.RunSourceGenerator(compilation2);

            AssertsModifiedOnLastStep(result2);
            result2.Diagnostics.Should().BeEmpty();
            AssertOutputs(result2, expected);
        }
    }
}
