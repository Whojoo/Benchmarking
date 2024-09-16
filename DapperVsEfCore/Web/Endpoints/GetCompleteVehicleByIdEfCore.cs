using Benchy.DapperVsEfCore.Database.Repositories;
using Benchy.DapperVsEfCore.Models;
using FastEndpoints;

namespace Web.Endpoints;

public record GetCompleteVehicleByIdEfCoreRespones(Vehicle Vehicle);

public class GetCompleteVehicleByIdEfCore(EfCoreDIRepository repository) : EndpointWithoutRequest<GetCompleteVehicleByIdEfCoreRespones>
{
    private readonly EfCoreDIRepository _repository = repository;

    public override void Configure()
    {
        Get("/ef/vehicles/{vehicleId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var vehicleId = Route<long>("vehicleId");
        var vehicle = await _repository.GetCompleteVehicleByIdAsync(vehicleId);
        var response = new GetCompleteVehicleByIdEfCoreRespones(vehicle);
        await SendAsync(response, cancellation: ct);
    }
}