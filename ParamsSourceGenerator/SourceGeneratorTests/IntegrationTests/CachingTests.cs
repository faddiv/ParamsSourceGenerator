﻿using FluentAssertions;
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

        public CachingTests()
        {
            GlobalSetup.Run();
            _compilerRunner = TestEnvironment.Compiler;
        }

        [Fact]
        public void Caches_When_Nothing_Changes()
        {
            var runner = new SourceGeneratorRunner<ParamsIncrementalGenerator>();
            var input = TestEnvironment.GetCachingSource();
            var expected = TestEnvironment.GetCachingOutputs();

            var compilation = _compilerRunner.CompileSources(input);

            var result1 = runner.RunSourceGenerator(compilation);

            var compilation2 = compilation.Clone();

            var result2 = runner.RunSourceGenerator(compilation2);

            AssertRunsEqual(result1, result2, _allTrackingNames);
            AssertAllOutputs(result2, IncrementalStepRunReason.Cached);
            result1.Diagnostics.Should().BeEmpty();
            AssertOutputsMatch(result1, expected);
        }

        [Fact]
        public void Caches_When_MethodBody_Changes()
        {
            var runner = new SourceGeneratorRunner<ParamsIncrementalGenerator>();
            var inputs = TestEnvironment.GetCachingSources();
            var expected = TestEnvironment.GetCachingOutputs();

            var compilation = _compilerRunner.CompileSources(inputs[0]);

            var result1 = runner.RunSourceGenerator(compilation);

            var compilation2 = _compilerRunner.CompileSources(inputs[1]);

            var result2 = runner.RunSourceGenerator(compilation2);

            AssertRunsEqual(result1, result2, _allTrackingNames);
            AssertAllOutputs(result2, IncrementalStepRunReason.Cached);
            result1.Diagnostics.Should().BeEmpty();
            AssertOutputsMatch(result1, expected);
        }

        [Fact]
        public void Regenerate_When_MaxOverrides_Changes()
        {
            var runner = new SourceGeneratorRunner<ParamsIncrementalGenerator>();
            var inputs = TestEnvironment.GetCachingSources();
            var expected = TestEnvironment.GetCachingOutputs();

            var compilation = _compilerRunner.CompileSources(inputs[0]);

            runner.RunSourceGenerator(compilation);

            var compilation2 = _compilerRunner.CompileSources(inputs[1]);

            var result2 = runner.RunSourceGenerator(compilation2);

            AssertAllOutputs(result2, IncrementalStepRunReason.Modified);
            result2.Diagnostics.Should().BeEmpty();
            AssertOutputsMatch(result2, expected);
        }

        [Fact]
        public void Regenerate_When_HasParams_Changes()
        {
            var runner = new SourceGeneratorRunner<ParamsIncrementalGenerator>();
            var inputs = TestEnvironment.GetCachingSources();
            var expected = TestEnvironment.GetCachingOutputs();

            var compilation = _compilerRunner.CompileSources(inputs[0]);

            runner.RunSourceGenerator(compilation);

            var compilation2 = _compilerRunner.CompileSources(inputs[1]);

            var result2 = runner.RunSourceGenerator(compilation2);

            AssertAllOutputs(result2, IncrementalStepRunReason.Modified);
            result2.Diagnostics.Should().BeEmpty();
            AssertOutputsMatch(result2, expected);
        }

        [Fact]
        public void Caches_When_OtherFileChanges()
        {
            var runner = new SourceGeneratorRunner<ParamsIncrementalGenerator>();
            var inputs = TestEnvironment.GetCachingSources();
            var expected = TestEnvironment.GetCachingOutputs();

            var compilation = _compilerRunner.CompileSources(inputs[0], inputs[1]);

            runner.RunSourceGenerator(compilation);

            var compilation2 = _compilerRunner.CompileSources(inputs[0], inputs[2]);

            var result2 = runner.RunSourceGenerator(compilation2);

            AssertOutputsMatch(result2, expected);
            AssertOutput(result2, "Something.Foo.g.cs", IncrementalStepRunReason.Cached);
            AssertOutput(result2, "Something.Baz.g.cs", IncrementalStepRunReason.Modified);
        }
    }
}
