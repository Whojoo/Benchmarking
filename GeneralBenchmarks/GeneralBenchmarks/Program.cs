// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using GeneralBenchmarks.MediatRClone;
using GeneralBenchmarks.ResultStructClass;

BenchmarkRunner.Run<ResultsBenchmark>();
// BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());