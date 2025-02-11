// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using GeneralBenchmarks;
using GeneralBenchmarks.Benchmarks;
using GeneralBenchmarks.FluentValidationBenchmark;

BenchmarkRunner.Run<FluentValidationBenchmark>();
