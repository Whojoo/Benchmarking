using Benchy.DapperVsEfCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Benchy.DapperVsEfCore.Database;

public static class VehicleDbSetExtensions
{
    public static IQueryable<Vehicle> GetIncludes(this DbSet<Vehicle> vehicleDbSet)
    {
        return vehicleDbSet
            .AsSplitQuery()
            .Include(vehicle => vehicle.EngineDetails)
            .Include(vehicle => vehicle.DamageImages)
            .Include(vehicle => vehicle.DetailImages)
            .Include(vehicle => vehicle.Model)
            .ThenInclude(model => model!.Make)
            .Include(vehicle => vehicle.Options)
            .Include(vehicle => vehicle.Tags)
            .Include(vehicle => vehicle.Thumbnail);
    }
}