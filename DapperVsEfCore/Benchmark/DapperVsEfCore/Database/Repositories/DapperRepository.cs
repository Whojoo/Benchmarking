using System.Diagnostics;
using Benchy.DapperVsEfCore.Database.Factories;
using Benchy.DapperVsEfCore.Models;
using Dapper;

namespace Benchy.DapperVsEfCore.Database.Repositories;

public class DapperRepository : IRepository
{
    public async Task<Vehicle?> GetSimpleVehicleByIdAsync(long vehicleId)
    {
        using var connection = ConnectionFactory.Create();
        return await connection.QueryFirstOrDefaultAsync<Vehicle>(
            DapperQueries.GetSimpleById,
            new { VehicleId = vehicleId });
    }

    public async Task<Vehicle?> GetCompleteVehicleByIdAsync(long vehicleId)
    {
        using var connection = ConnectionFactory.Create();

        await using var multiRead = await connection.QueryMultipleAsync(
            DapperQueries.GetCompleteById,
            new { VehicleId = vehicleId });

        var vehicle = multiRead
            .Read<Vehicle, EngineDetails, Model, Make, Image, Vehicle>(ProcessVehicleRead)
            .First();

        var options = multiRead.Read<Option>().ToList();
        var tags = multiRead.Read<Tag>().ToList();
        var images = multiRead.Read<Image>().ToList();
        var damageImages = multiRead.Read<DamageImage>().ToList();

        vehicle.Options = options;
        vehicle.Tags = tags;
        vehicle.DetailImages = images;
        vehicle.DamageImages = damageImages;

        return vehicle;
    }

    public async Task<VehiclesResult> GetSimpleVehiclesAsync(int page, int pageSize)
    {
        using var connection = ConnectionFactory.Create();

        await using var multiRead = await connection.QueryMultipleAsync(
            DapperQueries.GetSimpleVehicles,
            new { Skip = (page - 1) * pageSize, Take = pageSize });

        var vehicles = multiRead.Read<Vehicle>().ToList();
        var totalCount = multiRead.Read<int>().First();

        return new VehiclesResult(vehicles, totalCount);
    }

    public async Task<VehiclesResult> GetCompleteVehiclesAsync(int page, int pageSize)
    {
        using var connection = ConnectionFactory.Create();

        await using var multiRead = await connection.QueryMultipleAsync(
            DapperQueries.GetCompleteVehicles,
            new { Skip = (page - 1) * pageSize, Take = pageSize });

        var vehicles = multiRead
            .Read<Vehicle, EngineDetails, Model, Make, Image, Vehicle>(ProcessVehicleRead)
            .ToList();

        var images = multiRead.Read<Image>().ToList();
        var damageImages = multiRead.Read<DamageImage>().ToList();
        var optionTuples = multiRead
            .Read<long, Option, Tuple<long, Option>>(Tuple.Create)
            .ToList();
        var tagTuples = multiRead
            .Read<long, Tag, Tuple<long, Tag>>(Tuple.Create)
            .ToList();
        var totalCount = multiRead.Read<int>().First();

        var vehiclesDictionary = vehicles.ToDictionary(vehicle => vehicle.Id);

        foreach (var image in images)
        {
            vehiclesDictionary[image.VehicleId.Value].DetailImages.Add(image);
        }

        foreach (var damageImage in damageImages)
        {
            vehiclesDictionary[damageImage.VehicleId.Value].DamageImages.Add(damageImage);
        }

        foreach (var (vehicleId, option) in optionTuples)
        {
            vehiclesDictionary[vehicleId].Options.Add(option);
        }

        foreach (var (vehicleId, tag) in tagTuples)
        {
            vehiclesDictionary[vehicleId].Tags.Add(tag);
        }

        return new VehiclesResult(vehicles, totalCount);
    }

    private static Vehicle ProcessVehicleRead(
        Vehicle vehicle,
        EngineDetails engineDetails,
        Model model,
        Make? make,
        Image? thumbnail)
    {
        vehicle.EngineDetails = engineDetails;
        vehicle.Model = model;
        vehicle.Model.Make = make;
        vehicle.Thumbnail = thumbnail;
        return vehicle;
    }
}