using Benchy.DapperVsEfCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Benchy.DapperVsEfCore.Database.Repositories;

public class EfCoreRepository : IRepository
{
    public async Task<Vehicle?> GetSimpleVehicleByIdAsync(long vehicleId)
    {
        await using var vehicleDbContext = new VehicleDbContext();
        return await vehicleDbContext
            .Vehicles
            .Where(v => v.Id == vehicleId)
            .FirstOrDefaultAsync();
    }

    public async Task<Vehicle?> GetCompleteVehicleByIdAsync(long vehicleId)
    {
        await using var vehicleDbContext = new VehicleDbContext();
        return await vehicleDbContext
            .Vehicles
            .GetIncludes()
            .Where(v => v.Id == vehicleId)
            .FirstOrDefaultAsync();
    }

    public async Task<VehiclesResult> GetSimpleVehiclesAsync(int page, int pageSize)
    {
        await using var vehicleDbContext = new VehicleDbContext();
        var vehicles = await vehicleDbContext
            .Vehicles
            .OrderBy(vehicle => vehicle.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var total = await vehicleDbContext.Vehicles.CountAsync();
        
        return new VehiclesResult(vehicles, total);
    }

    public async Task<VehiclesResult> GetCompleteVehiclesAsync(int page, int pageSize)
    {
        await using var vehicleDbContext = new VehicleDbContext();
        var vehicles = await vehicleDbContext
            .Vehicles
            .GetIncludes()
            .OrderBy(vehicle => vehicle.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var total = await vehicleDbContext.Vehicles.CountAsync();

        return new VehiclesResult(vehicles, total);
    }
}