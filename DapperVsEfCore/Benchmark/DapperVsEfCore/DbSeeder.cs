using Benchy.DapperVsEfCore.Database;
using Benchy.DapperVsEfCore.Database.Factories;

namespace Benchy.DapperVsEfCore;

public static class DbSeeder
{
    public static async Task SeedAsync()
    {
        var vehiclesToAdd = DataFactory.GenerateVehicles();
        await using var vehicleDbContext = new VehicleDbContext();
        vehicleDbContext.AddRange(vehiclesToAdd);
        await vehicleDbContext.SaveChangesAsync();
    }
}