using Benchy.DapperVsEfCore.Models;

namespace Benchy.DapperVsEfCore.Database;

public interface IRepository
{
    Task<Vehicle?> GetSimpleVehicleByIdAsync(long vehicleId);
    Task<Vehicle?> GetCompleteVehicleByIdAsync(long vehicleId);
    Task<VehiclesResult> GetSimpleVehiclesAsync(int page, int pageSize);
    Task<VehiclesResult> GetCompleteVehiclesAsync(int page, int pageSize);
}