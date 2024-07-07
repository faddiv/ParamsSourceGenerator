using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
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

        [Fact]
        public async Task CanGenerate()
        {
            var runner = new SourceGeneratorTestRunner<ParamsIncrementalGenerator>();
            string input = TestEnvironment.GetCachingSource();
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
    }
}
