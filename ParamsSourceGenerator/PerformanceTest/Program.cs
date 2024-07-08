// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using PerformanceTest;

/*var c = new CodeGenerationBenchmark();
c.CreateBaseSource();
c.RunGenerator();*/

BenchmarkRunner.Run<CodeGenerationBenchmark>();