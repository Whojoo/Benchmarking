using System.Diagnostics;
using BenchmarkDotNet.Running;
using Benchy;
using Benchy.DapperVsEfCore;
using Benchy.DapperVsEfCore.Database.Factories;

BenchmarkRunner.Run<DapperVsEfCoreBenchmark>();

// var repo = RepositoryFactory.Dapper();
//
// foreach (var _ in Enumerable.Range(0, 1000))
// {
//     var stopwatch = Stopwatch.StartNew();
//     await repo.GetCompleteVehiclesAsync(15, 10);
//     var elapsedTotalMicroseconds = stopwatch.Elapsed.TotalMicroseconds;
//     Console.WriteLine(elapsedTotalMicroseconds);
// }
// // var vehicle = await repo.GetCompleteVehiclesAsync(15, 10);
// var vehicle = await repo.GetCompleteVehiclesAsync(15, 10);
