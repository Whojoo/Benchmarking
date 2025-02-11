using System.Diagnostics;
using BenchmarkDotNet.Running;
using Benchy;
using Benchy.DapperVsEfCore;
using Benchy.DapperVsEfCore.Database.Factories;

BenchmarkRunner.Run<DapperVsEfCoreBenchmark>();

// await DbSeeder.SeedAsync();

