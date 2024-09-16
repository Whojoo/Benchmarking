using Benchy.DapperVsEfCore.Database.Repositories;
using Benchy.DapperVsEfCore.Models;
using FastEndpoints;

namespace Web.Endpoints;

public record VehicleByIdEfCoreResponse(Vehicle Vehicle);

public class GetVehicleByIdEfCore(EfCoreDIRepository repository) : EndpointWithoutRequest<VehicleByIdEfCoreResponse>
{
    private readonly EfCoreDIRepository _repository = repository;

    public override void Configure()
    {
        Get("/ef/vehicles/{vehicleId}/simple");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var vehicleId = Route<long>("vehicleId");
        var vehicle = await _repository.GetSimpleVehicleByIdAsync(vehicleId);
        var response = new VehicleByIdEfCoreResponse(vehicle);
        await SendAsync(response, cancellation: ct);
    }
}