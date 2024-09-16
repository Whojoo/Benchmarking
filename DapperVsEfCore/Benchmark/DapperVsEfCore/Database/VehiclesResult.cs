using Benchy.DapperVsEfCore.Models;

namespace Benchy.DapperVsEfCore.Database;

public record VehiclesResult(List<Vehicle> Vehicles, int Total);