using System.Threading.Tasks;
using FluentAssertions;
using Foxy.Params.SourceGenerator.Data;
using Xunit;
using SourceGeneratorTests.TestInfrastructure;
using Foxy.Params.SourceGenerator;
using static SourceGeneratorTests.TestInfrastructure.CachingTestHelpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Foxy.Params;
using System.Text;

namespace SourceGeneratorTests.IntegrationTests
{
    public class CachingTests
    {

        // A collection of all the tracking names. I'll show how to simplify this later
        private static readonly string[] _allTrackingNames = [TrackingNames.GetSpanParamsMethods, TrackingNames.NotNullFilter];

        public CachingTests()
        {
            GlobalSetup.Run();
        }

        [Fact]
        public async Task Caches_When_Nothing_Changes()
        {
            var runner = await CreateTestRunner();
            var input = TestEnvironment.GetCachingSource();
            var expected = TestEnvironment.GetCachingOuputs();

            var compilation = runner.CompileSources(input);

            var result1 = runner.RunSourceGenerator(compilation);

            var compilation2 = compilation.Clone();

            var result2 = runner.RunSourceGenerator(compilation2);

            AssertRunsEqual(result1, result2, _allTrackingNames);
            AssertAllOutputs(result2, IncrementalStepRunReason.Cached);
            result1.Diagnostics.Should().BeEmpty();
            AssertOutputsMatch(result1, expected);
        }

        [Fact]
        public async Task Caches_When_MethodBody_Changes()
        {
            var runner = await CreateTestRunner();
            var inputs = TestEnvironment.GetCachingSources();
            var expected = TestEnvironment.GetCachingOuputs();

            var compilation = runner.CompileSources(inputs[0]);

            var result1 = runner.RunSourceGenerator(compilation);

            var compilation2 = runner.CompileSources(inputs[1]);

            var result2 = runner.RunSourceGenerator(compilation2);

            AssertRunsEqual(result1, result2, _allTrackingNames);
            AssertAllOutputs(result2, IncrementalStepRunReason.Cached);
            result1.Diagnostics.Should().BeEmpty();
            AssertOutputsMatch(result1, expected);
        }

        [Fact]
        public async Task Regenerate_When_MaxOverrides_Changes()
        {
            var runner = await CreateTestRunner();
            var inputs = TestEnvironment.GetCachingSources();
            var expected = TestEnvironment.GetCachingOuputs();

            var compilation = runner.CompileSources(inputs[0]);

            var result1 = runner.RunSourceGenerator(compilation);

            var compilation2 = runner.CompileSources(inputs[1]);

            var result2 = runner.RunSourceGenerator(compilation2);

            AssertAllOutputs(result2, IncrementalStepRunReason.Modified);
            result2.Diagnostics.Should().BeEmpty();
            AssertOutputsMatch(result2, expected);
        }

        [Fact]
        public async Task Regenerate_When_HasParams_Changes()
        {
            var runner = await CreateTestRunner();
            var inputs = TestEnvironment.GetCachingSources();
            var expected = TestEnvironment.GetCachingOuputs();

            var compilation = runner.CompileSources(inputs[0]);

            var result1 = runner.RunSourceGenerator(compilation);

            var compilation2 = runner.CompileSources(inputs[1]);

            var result2 = runner.RunSourceGenerator(compilation2);

            AssertAllOutputs(result2, IncrementalStepRunReason.Modified);
            result2.Diagnostics.Should().BeEmpty();
            AssertOutputsMatch(result2, expected);
        }

        [Fact]
        public async Task Caches_When_OtherFileChanges()
        {
            var runner = await CreateTestRunner();
            var inputs = TestEnvironment.GetCachingSources();
            var expected = TestEnvironment.GetCachingOuputs();

            var compilation = runner.CompileSources(inputs[0], inputs[1]);

            var result1 = runner.RunSourceGenerator(compilation);

            var compilation2 = runner.CompileSources(inputs[0], inputs[2]);

            var result2 = runner.RunSourceGenerator(compilation2);

            AssertOutputsMatch(result2, expected);
            AssertOutput(result2, "Something.Foo.g.cs", Microsoft.CodeAnalysis.IncrementalStepRunReason.Cached);
            AssertOutput(result2, "Something.Baz.g.cs", Microsoft.CodeAnalysis.IncrementalStepRunReason.Modified);
        }

        private static async Task<SourceGeneratorTestRunner<ParamsIncrementalGenerator>> CreateTestRunner()
        {
            var runner = new SourceGeneratorTestRunner<ParamsIncrementalGenerator>();
            await runner.LoadCSharpAssemblies();
            runner.AddAdditionalReference(typeof(ParamsAttribute).Assembly);
            return runner;
        }
    }
}
