using Benchy.DapperVsEfCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Benchy.DapperVsEfCore.Database.Repositories;

public class EfCoreRepository : IRepository
{
    public async Task<Vehicle?> GetSimpleVehicleByIdAsync(long vehicleId)
    {
        await using var vehicleDbContext = new VehicleDbContext();
        return await EfCoreStatic.GetSimpleVehicleByIdAsync(vehicleDbContext, vehicleId);
    }

    public async Task<Vehicle?> GetCompleteVehicleByIdAsync(long vehicleId)
    {
        await using var vehicleDbContext = new VehicleDbContext();
        return await EfCoreStatic.GetCompleteVehicleByIdAsync(vehicleDbContext, vehicleId);
    }

    public async Task<VehiclesResult> GetSimpleVehiclesAsync(int page, int pageSize)
    {
        await using var vehicleDbContext = new VehicleDbContext();
        return await EfCoreStatic.GetSimpleVehiclesAsync(vehicleDbContext, page, pageSize);
    }

    public async Task<VehiclesResult> GetCompleteVehiclesAsync(int page, int pageSize)
    {
        await using var vehicleDbContext = new VehicleDbContext();
        return await EfCoreStatic.GetCompleteVehiclesAsync(vehicleDbContext, page, pageSize);
    }
}