using Benchy.DapperVsEfCore.Database.Repositories;
using Benchy.DapperVsEfCore.Models;
using FastEndpoints;

namespace Web.Endpoints;

public class ListSimpleVehiclesEfCore(EfCoreDIRepository repository)
    : Endpoint<ListSimpleVehiclesEfCore.Request, ListSimpleVehiclesEfCore.Response>
{
    private readonly EfCoreDIRepository _repository = repository;

    public record Response(int TotalCount, List<Vehicle> Vehicles);
    public record Request(int? Page, int? PageSize);

    public override void Configure()
    {
        Get("/ef/vehicles/simple");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var page = req.Page ?? 1;
        var pageSize = req.PageSize ?? 10;
        var vehicles = await _repository.GetSimpleVehiclesAsync(page, pageSize);
        var response = new Response(vehicles.Total, vehicles.Vehicles);
        await SendAsync(response, cancellation: ct);
    }
}