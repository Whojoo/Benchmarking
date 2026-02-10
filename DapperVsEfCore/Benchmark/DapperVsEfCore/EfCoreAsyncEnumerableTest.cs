using BenchmarkDotNet.Attributes;
using Benchy.DapperVsEfCore.Database;
using Microsoft.EntityFrameworkCore;

namespace Benchy.DapperVsEfCore;

public record VehicleProjection(long Id, string MakeAndModel, decimal Price, string LicensePlate);

[MemoryDiagnoser]
public class EfCoreAsyncEnumerableTest
{
  private const int VehiclesToTake = 100;
  
  [Benchmark(Baseline = true)]
  public async Task<List<VehicleProjection>> GetVehiclesNormal()
  {
    await using var vehicleDbContext = new VehicleDbContext();
    return await vehicleDbContext
      .Vehicles
      .AsNoTracking()
      .Take(VehiclesToTake)
      .Select(x => new VehicleProjection(x.Id, x.Model!.Make!.Name + " " + x.Model.Name, x.Price, x.LicensePlate))
      .ToListAsync();
  }
  
  [Benchmark]
  public async Task<List<VehicleProjection>> GetVehiclesSplit()
  {
    await using var vehicleDbContext = new VehicleDbContext();
    var vehicles = await vehicleDbContext
      .Vehicles
      .AsNoTracking()
      .Select(x => new { x.Id, ModelName = x.Model!.Name, MakeName = x.Model!.Make!.Name, x.Price, x.LicensePlate })
      .Take(VehiclesToTake)
      .ToListAsync();

    return vehicles
      .Select(x => new VehicleProjection(x.Id, x.ModelName + " " + x.MakeName, x.Price, x.LicensePlate))
      .ToList();
  }
  
  [Benchmark]
  public async Task<List<VehicleProjection>> GetVehiclesAsyncLinq()
  {
    await using var vehicleDbContext = new VehicleDbContext();
    return await vehicleDbContext
      .Vehicles
      .AsNoTracking()
      .Select(x => new { x.Id, ModelName = x.Model!.Name, MakeName = x.Model!.Make!.Name, x.Price, x.LicensePlate })
      .Take(VehiclesToTake)
      .AsAsyncEnumerable()
      .Select(x => new VehicleProjection(x.Id, x.ModelName + " " + x.MakeName, x.Price, x.LicensePlate))
      .ToListAsync();
  }
}