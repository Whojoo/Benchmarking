using BenchmarkDotNet.Running;
using Benchy.DapperVsEfCore;

// BenchmarkRunner.Run<EfCoreAsyncEnumerableTest>();
BenchmarkRunner.Run<DapperGridReaderBenchmark>();
// BenchmarkRunner.Run<DapperVsEfCoreBenchmark>();

// await DbSeeder.SeedAsync();

