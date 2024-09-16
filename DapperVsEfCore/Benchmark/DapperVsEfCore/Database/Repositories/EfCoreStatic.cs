using Benchy.DapperVsEfCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Benchy.DapperVsEfCore.Database.Repositories;

public static class EfCoreStatic
{
    public static async Task<Vehicle?> GetSimpleVehicleByIdAsync(VehicleDbContext vehicleDbContext, long vehicleId)
    {
        return await vehicleDbContext
            .Vehicles
            .Where(v => v.Id == vehicleId)
            .FirstOrDefaultAsync();
    }

    public static async Task<Vehicle?> GetCompleteVehicleByIdAsync(VehicleDbContext vehicleDbContext, long vehicleId)
    {
        return await vehicleDbContext
            .Vehicles
            .GetIncludes()
            .Where(v => v.Id == vehicleId)
            .FirstOrDefaultAsync();
    }

    public static async Task<VehiclesResult> GetSimpleVehiclesAsync(VehicleDbContext vehicleDbContext, int page, int pageSize)
    {
        var vehicles = await vehicleDbContext
            .Vehicles
            .OrderBy(vehicle => vehicle.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var total = await vehicleDbContext.Vehicles.CountAsync();

        return new VehiclesResult(vehicles, total);
    }

    public static async Task<VehiclesResult> GetCompleteVehiclesAsync(VehicleDbContext vehicleDbContext, int page, int pageSize)
    {
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