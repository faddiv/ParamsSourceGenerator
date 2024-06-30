using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Foxy.Params.SourceGenerator.Data;
using Xunit;
using SourceGeneratorTests.TestInfrastructure;
using Foxy.Params.SourceGenerator;
using System.Collections;
using System.Reflection;

namespace SourceGeneratorTests.IntegrationTests
{
    public class CachingTests
    {

        // A collection of all the tracking names. I'll show how to simplify this later
        private static string[] AllTrackingNames = [TrackingNames.GetSpanParamsMethods, TrackingNames.NotNullFilter];

        [Fact]
        public void CanGenerate()
        {
            string input = TestEnvironment.GetCachingSource();
            var expected = TestEnvironment.GetCachingOuputs()[0];

            // run the generator, passing in the inputs and the tracking names
            var (diagnostics, output)
                = CachingTestHelpers.GetGeneratedTrees<ParamsIncrementalGenerator>([input], AllTrackingNames);

            // Assert the output
            using var s = new AssertionScope();
            diagnostics.Should().BeEmpty();
            output[0].Should().Be(expected.content);

        }

    }
}
