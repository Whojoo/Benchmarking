using BenchmarkDotNet.Attributes;
using Benchy.DapperVsEfCore.Database;
using Benchy.DapperVsEfCore.Database.Factories;
using Benchy.DapperVsEfCore.Models;

namespace Benchy.DapperVsEfCore;

[MemoryDiagnoser(false)]
public class DapperVsEfCoreBenchmark
{
    private const long IdToGet = 137;
    private const int BeginPage = 1;
    private const int BeginPageSize = 10;
    private const int MidPage = 12;
    private const int MidPageSize = 10;
    private const int EndPage = 25;
    private const int EndPageSize = 10;

    [Benchmark]
    public async Task<Vehicle?> GetSimpleVehicle_EfCore()
    {
        var repository = RepositoryFactory.EfCore();
        return await repository.GetSimpleVehicleByIdAsync(IdToGet);
    }

    [Benchmark]
    public async Task<Vehicle?> GetSimpleVehicle_Dapper()
    {
        var repository = RepositoryFactory.Dapper();
        return await repository.GetSimpleVehicleByIdAsync(IdToGet);
    }

    [Benchmark]
    public async Task<Vehicle?> GetCompleteVehicle_EfCore()
    {
        var repository = RepositoryFactory.EfCore();
        return await repository.GetCompleteVehicleByIdAsync(IdToGet);
    }

    [Benchmark]
    public async Task<Vehicle?> GetCompleteVehicle_Dapper()
    {
        var repository = RepositoryFactory.Dapper();
        return await repository.GetCompleteVehicleByIdAsync(IdToGet);
    }

    [Benchmark]
    public async Task<VehiclesResult> GetSimpleVehicles_Begin_EfCore()
    {
        var repository = RepositoryFactory.EfCore();
        return await repository.GetSimpleVehiclesAsync(BeginPage, BeginPageSize);
    }

    [Benchmark]
    public async Task<VehiclesResult> GetSimpleVehicles_Begin_Dapper()
    {
        var repository = RepositoryFactory.Dapper();
        return await repository.GetSimpleVehiclesAsync(BeginPage, BeginPageSize);
    }

    [Benchmark]
    public async Task<VehiclesResult> GetCompleteVehicles_Begin_EfCore()
    {
        var repository = RepositoryFactory.EfCore();
        return await repository.GetCompleteVehiclesAsync(BeginPage, BeginPageSize);
    }

    [Benchmark]
    public async Task<VehiclesResult> GetCompleteVehicles_Begin_Dapper()
    {
        var repository = RepositoryFactory.Dapper();
        return await repository.GetCompleteVehiclesAsync(BeginPage, BeginPageSize);
    }

    [Benchmark]
    public async Task<VehiclesResult> GetSimpleVehicles_Mid_EfCore()
    {
        var repository = RepositoryFactory.EfCore();
        return await repository.GetSimpleVehiclesAsync(MidPage, MidPageSize);
    }

    [Benchmark]
    public async Task<VehiclesResult> GetSimpleVehicles_Mid_Dapper()
    {
        var repository = RepositoryFactory.Dapper();
        return await repository.GetSimpleVehiclesAsync(MidPage, MidPageSize);
    }

    [Benchmark]
    public async Task<VehiclesResult> GetCompleteVehicles_Mid_EfCore()
    {
        var repository = RepositoryFactory.EfCore();
        return await repository.GetCompleteVehiclesAsync(MidPage, MidPageSize);
    }

    [Benchmark]
    public async Task<VehiclesResult> GetCompleteVehicles_Mid_Dapper()
    {
        var repository = RepositoryFactory.Dapper();
        return await repository.GetCompleteVehiclesAsync(MidPage, MidPageSize);
    }

    [Benchmark]
    public async Task<VehiclesResult> GetSimpleVehicles_End_EfCore()
    {
        var repository = RepositoryFactory.EfCore();
        return await repository.GetSimpleVehiclesAsync(EndPage, EndPageSize);
    }

    [Benchmark]
    public async Task<VehiclesResult> GetSimpleVehicles_End_Dapper()
    {
        var repository = RepositoryFactory.Dapper();
        return await repository.GetSimpleVehiclesAsync(EndPage, EndPageSize);
    }

    [Benchmark]
    public async Task<VehiclesResult> GetCompleteVehicles_End_EfCore()
    {
        var repository = RepositoryFactory.EfCore();
        return await repository.GetCompleteVehiclesAsync(EndPage, EndPageSize);
    }

    [Benchmark]
    public async Task<VehiclesResult> GetCompleteVehicles_End_Dapper()
    {
        var repository = RepositoryFactory.Dapper();
        return await repository.GetCompleteVehiclesAsync(EndPage, EndPageSize);
    }
}