// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using GeneralBenchmarks;
using GeneralBenchmarks.Benchmarks;

BenchmarkRunner.Run<MapJsonOrExtensionBenchmarks>();
