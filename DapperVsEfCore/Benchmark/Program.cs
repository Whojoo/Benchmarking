using BenchmarkDotNet.Running;
using Benchy.DapperVsEfCore;

BenchmarkRunner.Run<DapperVsEfCoreBenchmark>();

// await DbSeeder.SeedAsync();

