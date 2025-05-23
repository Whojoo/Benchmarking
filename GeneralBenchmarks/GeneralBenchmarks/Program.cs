// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using GeneralBenchmarks.MediatRClone;

BenchmarkRunner.Run<MediatRAlternativeBenchmark>();
// BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());