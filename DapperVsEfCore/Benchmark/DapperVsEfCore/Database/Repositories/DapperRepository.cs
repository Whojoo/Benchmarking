using Benchy.DapperVsEfCore.Database.Factories;
using Benchy.DapperVsEfCore.Models;
using Dapper;

namespace Benchy.DapperVsEfCore.Database.Repositories;

public class DapperRepository(string? connectionString = null) : IRepository
{
    private readonly string _connectionString = connectionString ?? DataSchemaConstants.ConnectionString;

    public async Task<Vehicle?> GetSimpleVehicleByIdAsync(long vehicleId)
    {
        using var connection = await ConnectionFactory.Create(_connectionString);
        const string sql =
            """
            SELECT v.*
            FROM DapperVsEfCore.Vehicles v
            WHERE v.Id = @VehicleId
            """;
        return await connection.QueryFirstOrDefaultAsync<Vehicle>(sql, new { VehicleId = vehicleId });
    }

    public async Task<Vehicle?> GetCompleteVehicleByIdAsync(long vehicleId)
    {
        using var connection = await ConnectionFactory.Create(_connectionString);

        const string sql =
            """
            SELECT v.*, e.*, model.*, make.*, image.*
            FROM DapperVsEfCore.Vehicles v
            INNER JOIN DapperVsEfCore.EngineDetails e ON v.EngineDetailsId = e.Id
            INNER JOIN DapperVsEfCore.Models model ON model.Id = v.ModelId
            INNER JOIN DapperVsEfCore.Makes make ON make.Id = model.MakeId
            INNER JOIN DapperVsEfCore.Images image ON image.Id = v.ThumbnailId
            WHERE v.Id = @VehicleId

            SELECT o.*
            FROM DapperVsEfCore.Options o
            INNER JOIN DapperVsEfCore.OptionVehicle ov ON o.Id = ov.OptionsId
            WHERE ov.VehicleId = @VehicleId

            SELECT t.*
            FROM DapperVsEfCore.Tags t
            INNER JOIN DapperVsEfCore.TagVehicle tv ON t.Id = tv.TagsId
            WHERE tv.VehicleId = @VehicleId

            SELECT i.*
            FROM DapperVsEfCore.Images i
            WHERE i.VehicleId = @VehicleId

            SELECT di.*
            FROM DapperVsEfCore.DamageImage di
            WHERE di.VehicleId = @VehicleId
            """;

        await using var multiRead = await connection.QueryMultipleAsync(sql, new { VehicleId = vehicleId });

        var vehicle = multiRead
            .Read<Vehicle, EngineDetails, Model, Make, Image, Vehicle>(ProcessVehicleRead)
            .FirstOrDefault();

        if (vehicle is null)
            return null;

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
        using var connection = await ConnectionFactory.Create(_connectionString);

        const string sql =
            """
            SELECT v.*
            FROM DapperVsEfCore.Vehicles v
            ORDER BY v.Id
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY

            SELECT COUNT(*)
            FROM DapperVsEfCore.Vehicles
            """;

        await using var multiRead = await connection.QueryMultipleAsync(
            sql,
            new { Skip = (page - 1) * pageSize, Take = pageSize });

        var vehicles = multiRead.Read<Vehicle>().ToList();
        var totalCount = multiRead.Read<int>().First();

        return new VehiclesResult(vehicles, totalCount);
    }

    public async Task<VehiclesResult> GetCompleteVehiclesAsync(int page, int pageSize)
    {
        using var connection = await ConnectionFactory.Create(_connectionString);

        const string sql =
            """
            SELECT v.*, e.*, model.*, make.*, image.*
            FROM DapperVsEfCore.Vehicles v
            INNER JOIN DapperVsEfCore.EngineDetails e ON v.EngineDetailsId = e.Id
            INNER JOIN DapperVsEfCore.Models model ON model.Id = v.ModelId
            INNER JOIN DapperVsEfCore.Makes make ON make.Id = model.MakeId
            INNER JOIN DapperVsEfCore.Images image ON image.Id = v.ThumbnailId
            ORDER BY v.Id
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY

            SELECT image.*
            FROM DapperVsEfCore.Images image
            WHERE image.VehicleId IN (SELECT Id
                                      FROM DapperVsEfCore.Vehicles
                                      ORDER BY Id
                                      OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY)

            SELECT damageImage.*
            FROM DapperVsEfCore.DamageImage damageImage
            WHERE damageImage.VehicleId IN (SELECT Id
                                            FROM DapperVsEfCore.Vehicles
                                            ORDER BY Id
                                            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY)

            SELECT ov.VehicleId, o.*
            FROM DapperVsEfCore.Options o
            INNER JOIN DapperVsEfCore.OptionVehicle ov ON o.Id = ov.OptionsId
            WHERE ov.VehicleId IN (SELECT Id
                                   FROM DapperVsEfCore.Vehicles
                                   ORDER BY Id
                                   OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY)   
                                   
            SELECT tv.VehicleId, t.*
            FROM DapperVsEfCore.Tags t
            INNER JOIN DapperVsEfCore.TagVehicle tv ON t.Id = tv.TagsId
            WHERE tv.VehicleId IN (SELECT Id
                                   FROM DapperVsEfCore.Vehicles
                                   ORDER BY Id
                                   OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY)   
                                   
            SELECT COUNT(*)
            FROM DapperVsEfCore.Vehicles
            """;

        await using var multiRead = await connection.QueryMultipleAsync(
            sql,
            new { Skip = (page - 1) * pageSize, Take = pageSize });

        var vehiclesDictionary = multiRead
            .Read<Vehicle, EngineDetails, Model, Make, Image, Vehicle>(ProcessVehicleRead)
            .ToDictionary(vehicle => vehicle.Id);

        var images = multiRead.Read<Image>().ToList();
        var damageImages = multiRead.Read<DamageImage>().ToList();
        var optionTuples = multiRead
            .Read<long, Option, Tuple<long, Option>>(Tuple.Create)
            .ToList();
        var tagTuples = multiRead
            .Read<long, Tag, Tuple<long, Tag>>(Tuple.Create)
            .ToList();
        var totalCount = multiRead.Read<int>().First();

        foreach (var image in images)
            if (vehiclesDictionary.TryGetValue(image.VehicleId ?? 0, out var vehicle))
                vehicle.DetailImages.Add(image);
        
        foreach (var damageImage in damageImages)
            if (vehiclesDictionary.TryGetValue(damageImage.VehicleId ?? 0, out var vehicle))
                vehicle.DamageImages.Add(damageImage);

        foreach (var (vehicleId, option) in optionTuples)
            if (vehiclesDictionary.TryGetValue(vehicleId, out var vehicle))
                vehicle.Options.Add(option);

        foreach (var (vehicleId, tag) in tagTuples)
            if (vehiclesDictionary.TryGetValue(vehicleId, out var vehicle))
                vehicle.Tags.Add(tag);

        return new VehiclesResult(vehiclesDictionary.Values.ToList(), totalCount);
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