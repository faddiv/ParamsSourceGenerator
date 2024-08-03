// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using PerformanceTest;

var config = new ManualConfig()
    .WithOptions(ConfigOptions.DisableOptimizationsValidator)
    .AddValidator(JitOptimizationsValidator.FailOnError)
    .AddLogger(ConsoleLogger.Default)
    .AddColumnProvider(DefaultColumnProviders.Instance);

/*var c = new CodeGenerationBenchmark();
c.CreateBaseSource();
c.RunGenerator();*/

/*var c = new ChangeTrackingBenchmark();
await c.CreateBaseSource();
c.OnlyOneFileChanges();
c.OnlyOneFileChanges();
c.OnlyOneFileChanges();*/

/*
var c = new SourceBuilderBenchmark();
Console.WriteLine(c.InterpolatedStringHandler());
/**/

/*
var c = new SemanticHelpersBenchmark();
c.Setup();
Console.WriteLine(string.Join(", ", c.GetTypeHierarchyV1()));
Console.WriteLine(string.Join(", ", c.GetTypeHierarchyV2()));
Console.WriteLine(string.Join(", ", c.GetTypeHierarchyV3()));
/**/

/*var c = new SuccessfulParamsCandidateCreationBenchmark();
c.Setup();
var result = c.CreateSuccessfulParamsCandidate();
Console.WriteLine(result);
*/

BenchmarkRunner.Run<SuccessfulParamsCandidateCreationBenchmark>(config);
