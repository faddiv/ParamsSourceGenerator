using FluentAssertions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;
using System.Collections;

namespace SourceGeneratorTests.TestInfrastructure;

internal static partial class CachingTestHelpers
{
    public static void AssertOutputsMatch(GeneratorDriverRunResult runResult, ICollection<CSharpFile> files)
    {
        var actualFileNames = runResult.GeneratedTrees.Select(e => Path.GetFileName(e.FilePath));
        var expectedFileNames = files.Select(e => e.Name);

        actualFileNames.Should().Contain(expectedFileNames);
        expectedFileNames.Should().Contain(actualFileNames);

        foreach (var file in files)
        {
            var tree = runResult.GeneratedTrees.First(e => Path.GetFileName(e.FilePath) == file.Name);
            tree.ToString().Should().Be(file.Content);
        }
    }

    public static void AssertAllOutputs(GeneratorDriverRunResult runResult, IncrementalStepRunReason reason)
    {
        // verify the second run only generated cached source outputs
        runResult.Results[0]
                    .TrackedOutputSteps
                    .SelectMany(x => x.Value) // step executions
                    .SelectMany(x => x.Outputs) // execution results
                    .Should()
                    .HaveCountGreaterThan(0)
                    .And
                    .OnlyContain(x => x.Reason == reason);
    }

    public static void AssertOutput(GeneratorDriverRunResult runResult, string outputName, IncrementalStepRunReason reason)
    {
        var concreteOutputs = runResult.Results[0].TrackedOutputSteps
                    .SelectMany(x => x.Value) // step executions
                    .SelectMany(x => x.Outputs) // execution results
                    .Select(x => new { x.Reason, Values = ExtractGeneratedSourceTexts(x) })
                    .SelectMany(x => x.Values.Select(e => new { x.Reason, Value = e }))
                    .Where(x => GetDynamicValue<string>(x.Value, "HintName") == outputName)
                    .ToList();
        concreteOutputs.Should().HaveCount(1).And.OnlyContain(x => x.Reason == reason);
    }

    public static void AssertRunsEqual(
        GeneratorDriverRunResult runResult1,
        GeneratorDriverRunResult runResult2,
        string[] trackingNames)
    {
        // We're given all the tracking names, but not all the
        // stages will necessarily execute, so extract all the 
        // output steps, and filter to ones we know about
        var trackedSteps1 = GetTrackedSteps(runResult1, trackingNames);
        var trackedSteps2 = GetTrackedSteps(runResult2, trackingNames);

        // Both runs should have the same tracked steps
        trackedSteps1.Should()
                     .NotBeEmpty()
                     .And.HaveSameCount(trackedSteps2)
                     .And.ContainKeys(trackedSteps2.Keys);

        // Get the IncrementalGeneratorRunStep collection for each run
        foreach (var (trackingName, runSteps1) in trackedSteps1)
        {
            // Assert that both runs produced the same outputs
            var runSteps2 = trackedSteps2[trackingName];
            AssertEqual(runSteps1, runSteps2, trackingName);
        }

        return;

        // Local function that extracts the tracked steps
        static Dictionary<string, ImmutableArray<IncrementalGeneratorRunStep>> GetTrackedSteps(
            GeneratorDriverRunResult runResult, string[] trackingNames)
            => runResult
                    .Results[0] // We're only running a single generator, so this is safe
                    .TrackedSteps // Get the pipeline outputs
                    .Where(step => trackingNames.Contains(step.Key)) // filter to known steps
                    .ToDictionary(x => x.Key, x => x.Value); // Convert to a dictionary
    }

    private static IEnumerable<object> ExtractGeneratedSourceTexts((object Value, IncrementalStepRunReason Reason) x)
    {
        return x.Value is ITuple tuple
            && tuple[0] is IList list
            ? list.Cast<object>()
            : [];
    }

    private static T? GetDynamicValue<T>(object? value, string propertyName)
    {
        if (value is null)
        {
            return default;
        }

        var type = value.GetType();
        var property = type.GetProperty(propertyName);
        if (property is null)
        {
            throw new ApplicationException($"Property {propertyName} doesn't exists on {type.FullName}");
        }
        return (T?)property.GetValue(value);
    }

    private static void AssertEqual(
        ImmutableArray<IncrementalGeneratorRunStep> runSteps1,
        ImmutableArray<IncrementalGeneratorRunStep> runSteps2,
        string stepName)
    {
        runSteps1.Should().HaveSameCount(runSteps2);

        for (var i = 0; i < runSteps1.Length; i++)
        {
            var runStep1 = runSteps1[i];
            var runStep2 = runSteps2[i];

            // The outputs should be equal between different runs
            IEnumerable<object> outputs1 = runStep1.Outputs.Select(x => x.Value);
            IEnumerable<object> outputs2 = runStep2.Outputs.Select(x => x.Value);

            outputs1.Should()
                    .Equal(outputs2, $"because {stepName} should produce cacheable outputs");

            // Therefore, on the second run the results should always be cached or unchanged!
            // - Unchanged is when the _input_ has changed, but the output hasn't
            // - Cached is when the the input has not changed, so the cached output is used 
            runStep2.Outputs.Should()
                .OnlyContain(
                    x => x.Reason == IncrementalStepRunReason.Cached || x.Reason == IncrementalStepRunReason.Unchanged,
                    $"{stepName} expected to have reason {IncrementalStepRunReason.Cached} or {IncrementalStepRunReason.Unchanged}");
        }
    }
}
