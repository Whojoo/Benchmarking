using Benchy.DapperVsEfCore.Database.Repositories;
using Benchy.DapperVsEfCore.Models;
using FastEndpoints;

namespace Web.Endpoints;

public record GetVehicleByIdDapperResponse(Vehicle Vehicle);

public class GetVehicleByIdDapper(DapperRepository dapperRepository) : EndpointWithoutRequest<GetVehicleByIdDapperResponse>
{
    private readonly DapperRepository _dapperRepository = dapperRepository;

    public override void Configure()
    {
        Get("/dapper/vehicles/{vehicleId}/simple");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var vehicleId = Route<long>("vehicleId");
        var vehicle = await _dapperRepository.GetSimpleVehicleByIdAsync(vehicleId);
        var response = new GetVehicleByIdDapperResponse(vehicle);
        await SendAsync(response, cancellation: ct);
    }
}