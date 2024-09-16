using Benchy.DapperVsEfCore.Database.Repositories;
using Benchy.DapperVsEfCore.Models;
using FastEndpoints;

namespace Web.Endpoints;

public class ListSimpleVehiclesDapper(DapperRepository repository)
    : Endpoint<ListSimpleVehiclesDapper.Request, ListSimpleVehiclesDapper.Response>
{
    private readonly DapperRepository _repository = repository;

    public record Response(int TotalCount, List<Vehicle> Vehicles);

    public record Request(int? Page, int? PageSize);

    public override void Configure()
    {
        Get("/dapper/vehicles/simple");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var page = req.Page ?? 1;
        var pageSize = req.PageSize ?? 10;

        var vehicles = await _repository.GetSimpleVehiclesAsync(page, pageSize);
        await SendAsync(new Response(vehicles.Total, vehicles.Vehicles), cancellation: ct);
    }
}