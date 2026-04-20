using Foxy.Params.SourceGenerator.Data;
using Xunit;
using SourceGeneratorTests.TestInfrastructure;
using Foxy.Params.SourceGenerator;
using static SourceGeneratorTests.TestInfrastructure.CachingTestHelpers;
using Microsoft.CodeAnalysis;
using Test.Infrastructure;

namespace SourceGeneratorTests.IntegrationTests
{
    public class CachingTests
    {
        // A collection of all the tracking names. I'll show how to simplify this later
        private static readonly string[] _allTrackingNames = [TrackingNames.GetSpanParamsMethods, TrackingNames.NotNullFilter];
        private readonly CompilerRunner _compilerRunner;
        private readonly TestEnvironment _testEnvironment;

        public CachingTests(TestEnvironment testEnvironment)
        {
            _testEnvironment = testEnvironment;
            _compilerRunner = _testEnvironment.Compiler;
        }

        [Fact]
        public void Caches_When_Nothing_Changes()
        {
            var runner = new SourceGeneratorRunner<ParamsIncrementalGenerator>();
            var input = _testEnvironment.GetCachingSource();
            var expected = _testEnvironment.GetCachingOutputs();

            var compilation = _compilerRunner.CompileSources([input], TestContext.Current.CancellationToken);

            var result1 = runner.RunSourceGenerator(compilation, TestContext.Current.CancellationToken);

            var compilation2 = compilation.Clone();

            var result2 = runner.RunSourceGenerator(compilation2, TestContext.Current.CancellationToken);

            AssertRunsEqual(result1, result2, _allTrackingNames);
            AssertAllOutputs(result2, IncrementalStepRunReason.Cached);
            Assert.Empty(result1.Diagnostics);
            AssertOutputsMatch(result1, expected);
        }

        [Fact]
        public void Caches_When_MethodBody_Changes()
        {
            var runner = new SourceGeneratorRunner<ParamsIncrementalGenerator>();
            var inputs = _testEnvironment.GetCachingSources();
            var expected = _testEnvironment.GetCachingOutputs();

            var compilation = _compilerRunner.CompileSources([inputs[0]], TestContext.Current.CancellationToken);

            var result1 = runner.RunSourceGenerator(compilation, TestContext.Current.CancellationToken);

            var compilation2 = _compilerRunner.CompileSources([inputs[1]], TestContext.Current.CancellationToken);

            var result2 = runner.RunSourceGenerator(compilation2, TestContext.Current.CancellationToken);

            AssertRunsEqual(result1, result2, _allTrackingNames);
            AssertAllOutputs(result2, IncrementalStepRunReason.Cached);
            Assert.Empty(result2.Diagnostics);
            AssertOutputsMatch(result1, expected);
        }

        [Fact]
        public void Regenerate_When_MaxOverrides_Changes()
        {
            var runner = new SourceGeneratorRunner<ParamsIncrementalGenerator>();
            var inputs = _testEnvironment.GetCachingSources();
            var expected = _testEnvironment.GetCachingOutputs();

            var compilation = _compilerRunner.CompileSources([inputs[0]], TestContext.Current.CancellationToken);

            runner.RunSourceGenerator(compilation, TestContext.Current.CancellationToken);

            var compilation2 = _compilerRunner.CompileSources([inputs[1]], TestContext.Current.CancellationToken);

            var result2 = runner.RunSourceGenerator(compilation2, TestContext.Current.CancellationToken);

            AssertAllOutputs(result2, IncrementalStepRunReason.Modified);
            Assert.Empty(result2.Diagnostics);
            AssertOutputsMatch(result2, expected);
        }

        [Fact]
        public void Regenerate_When_HasParams_Changes()
        {
            var runner = new SourceGeneratorRunner<ParamsIncrementalGenerator>();
            var inputs = _testEnvironment.GetCachingSources();
            var expected = _testEnvironment.GetCachingOutputs();

            var compilation = _compilerRunner.CompileSources([inputs[0]], TestContext.Current.CancellationToken);

            runner.RunSourceGenerator(compilation, TestContext.Current.CancellationToken);

            var compilation2 = _compilerRunner.CompileSources([inputs[1]], TestContext.Current.CancellationToken);

            var result2 = runner.RunSourceGenerator(compilation2, TestContext.Current.CancellationToken);

            AssertAllOutputs(result2, IncrementalStepRunReason.Modified);
            Assert.Empty(result2.Diagnostics);
            AssertOutputsMatch(result2, expected);
        }

        [Fact]
        public void Caches_When_OtherFileChanges()
        {
            var runner = new SourceGeneratorRunner<ParamsIncrementalGenerator>();
            var inputs = _testEnvironment.GetCachingSources();
            var expected = _testEnvironment.GetCachingOutputs();

            var compilation = _compilerRunner.CompileSources([inputs[0], inputs[1]], TestContext.Current.CancellationToken);

            runner.RunSourceGenerator(compilation, TestContext.Current.CancellationToken);

            var compilation2 = _compilerRunner.CompileSources([inputs[0], inputs[2]], TestContext.Current.CancellationToken);

            var result2 = runner.RunSourceGenerator(compilation2, TestContext.Current.CancellationToken);

            AssertOutputsMatch(result2, expected);
            AssertOutput(result2, "Something.Foo.g.cs", IncrementalStepRunReason.Cached);
            AssertOutput(result2, "Something.Baz.g.cs", IncrementalStepRunReason.Modified);
        }
    }
}
