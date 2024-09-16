using Benchy.DapperVsEfCore.Database.Repositories;
using Benchy.DapperVsEfCore.Models;
using FastEndpoints;
using FluentValidation;

namespace Web.Endpoints;

public class ListCompleteVehiclesDapper(DapperRepository repository)
    : Endpoint<ListCompleteVehiclesDapper.Request, ListCompleteVehiclesDapper.Response>
{
    private readonly DapperRepository _repository = repository;

    public record Response(int TotalCount, List<Vehicle> Vehicles);

    public record Request(int Page = 1, int PageSize = 10);

    public override void Configure()
    {
        Get("/dapper/vehicles");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var vehicles = await _repository.GetCompleteVehiclesAsync(req.Page, req.PageSize);
        await SendAsync(new Response(vehicles.Total, vehicles.Vehicles), cancellation: ct);
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1);
        }
    }
}