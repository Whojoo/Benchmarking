using Benchy.DapperVsEfCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Benchy.DapperVsEfCore.Database.Repositories;

public class EfCoreDIRepository(VehicleDbContext vehicleDbContext) : IRepository
{
    private readonly VehicleDbContext _vehicleDbContext = vehicleDbContext;

    public async Task<Vehicle?> GetSimpleVehicleByIdAsync(long vehicleId)
    {
        return await EfCoreStatic.GetSimpleVehicleByIdAsync(_vehicleDbContext, vehicleId);
    }

    public async Task<Vehicle?> GetCompleteVehicleByIdAsync(long vehicleId)
    {
        return await EfCoreStatic.GetCompleteVehicleByIdAsync(_vehicleDbContext, vehicleId);
    }

    public async Task<VehiclesResult> GetSimpleVehiclesAsync(int page, int pageSize)
    {
        return await EfCoreStatic.GetSimpleVehiclesAsync(_vehicleDbContext, page, pageSize);
    }

    public async Task<VehiclesResult> GetCompleteVehiclesAsync(int page, int pageSize)
    {
        return await EfCoreStatic.GetCompleteVehiclesAsync(_vehicleDbContext, page, pageSize);
    }
}