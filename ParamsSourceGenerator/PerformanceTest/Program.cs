// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using PerformanceTest;

/*var c = new CodeGenerationBenchmark();
c.CreateBaseSource();
c.RunGenerator();*/

/*var c = new ChangeTrackingBenchmark();
await c.CreateBaseSource();
c.OnlyOneFileChanges();
c.OnlyOneFileChanges();
c.OnlyOneFileChanges();*/

var config = new ManualConfig()
    .WithOptions(ConfigOptions.DisableOptimizationsValidator)
    .AddValidator(JitOptimizationsValidator.FailOnError)
    .AddLogger(ConsoleLogger.Default)
    .AddColumnProvider(DefaultColumnProviders.Instance);

BenchmarkRunner.Run<ChangeTrackingBenchmark>(config);
