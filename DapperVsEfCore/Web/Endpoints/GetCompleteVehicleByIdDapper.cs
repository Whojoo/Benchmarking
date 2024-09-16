using Benchy.DapperVsEfCore.Database.Repositories;
using Benchy.DapperVsEfCore.Models;
using FastEndpoints;

namespace Web.Endpoints;

public record GetCompleteVehicleByIdDapperResponse(Vehicle Vehicle);

public class GetCompleteVehicleByIdDapper(DapperRepository dapperRepository) : EndpointWithoutRequest<GetCompleteVehicleByIdDapperResponse>
{
    private readonly DapperRepository _dapperRepository = dapperRepository;

    public override void Configure()
    {
        Get("/dapper/vehicles/{vehicleId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var vehicleId = Route<long>("vehicleId");
        var vehicle = await _dapperRepository.GetCompleteVehicleByIdAsync(vehicleId);
        var response = new GetCompleteVehicleByIdDapperResponse(vehicle);
        await SendAsync(response, cancellation: ct);
    }
}