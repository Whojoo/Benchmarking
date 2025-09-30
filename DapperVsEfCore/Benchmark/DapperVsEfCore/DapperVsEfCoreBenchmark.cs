using BenchmarkDotNet.Attributes;
using Benchy.DapperVsEfCore.Database;
using Benchy.DapperVsEfCore.Database.Factories;
using Benchy.DapperVsEfCore.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Benchy.DapperVsEfCore;

[MemoryDiagnoser]
public class DapperVsEfCoreBenchmark
{
    private const long IdToGet = 137;
    private const int MidPage = 12;
    private const int MidPageSize = 10;

    [Benchmark]
    public async Task<Vehicle?> GetSimpleVehicle_EfCore()
    {
        await using var vehicleDbContext = new VehicleDbContext();
        return await vehicleDbContext
            .Vehicles
            .Where(v => v.Id == IdToGet)
            .FirstOrDefaultAsync();
    }

    [Benchmark]
    public async Task<Vehicle?> GetCompleteVehicle_EfCore()
    {
        await using var vehicleDbContext = new VehicleDbContext();
        return await vehicleDbContext
            .Vehicles
            .GetIncludes()
            .Where(v => v.Id == IdToGet)
            .FirstOrDefaultAsync();
    }

    [Benchmark]
    public async Task<VehiclesResult> GetSimpleVehicles_Mid_EfCore()
    {
        await using var vehicleDbContext = new VehicleDbContext();
        var vehicles = await vehicleDbContext
            .Vehicles
            .OrderBy(vehicle => vehicle.Id)
            .Skip((MidPage - 1) * MidPageSize)
            .Take(MidPageSize)
            .ToListAsync();

        var total = await vehicleDbContext.Vehicles.CountAsync();
        
        return new VehiclesResult(vehicles, total);
    }

    [Benchmark]
    public async Task<VehiclesResult> GetCompleteVehicles_Mid_EfCore()
    {
        await using var vehicleDbContext = new VehicleDbContext();
        var vehicles = await vehicleDbContext
            .Vehicles
            .GetIncludes()
            .OrderBy(vehicle => vehicle.Id)
            .Skip((MidPage - 1) * MidPageSize)
            .Take(MidPageSize)
            .ToListAsync();

        var total = await vehicleDbContext.Vehicles.CountAsync();
        
        return new VehiclesResult(vehicles, total);
    }

    [Benchmark]
    public async Task<VehiclesResult> GetSimpleVehicles_EfCore()
    {
        await using var vehicleDbContext = new VehicleDbContext();
        var vehicles = await vehicleDbContext
            .Vehicles
            .ToListAsync();
        
        var total = await vehicleDbContext.Vehicles.CountAsync();
        
        return new VehiclesResult(vehicles, total);
    }

    [Benchmark]
    public async Task<VehiclesResult> GetCompleteVehicles_EfCore()
    {
        await using var vehicleDbContext = new VehicleDbContext();
        var vehicles = await vehicleDbContext
            .Vehicles
            .GetIncludes()
            .ToListAsync();
        
        var total = await vehicleDbContext.Vehicles.CountAsync();
        
        return new VehiclesResult(vehicles, total);
    }

    [Benchmark]
    public async Task<Vehicle?> GetSimpleVehicle_Dapper()
    {
        using var connection = await ConnectionFactory.Create(DataSchemaConstants.ConnectionString);
        const string sql =
            """
            SELECT v.*
            FROM DapperVsEfCore.Vehicles v
            WHERE v.Id = @VehicleId
            """;
        return await connection.QueryFirstOrDefaultAsync<Vehicle>(sql, new { VehicleId = IdToGet });
    }

    [Benchmark]
    public async Task<Vehicle?> GetCompleteVehicle_Dapper()
    {
        using var connection = await ConnectionFactory.Create(DataSchemaConstants.ConnectionString);

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

        await using var multiRead = await connection.QueryMultipleAsync(sql, new { VehicleId = IdToGet });

        var vehicle = multiRead
            .Read<Vehicle, EngineDetails, Model, Make, Image, Vehicle>((vehicle1, details, model, make, image) =>
            {
                vehicle1.EngineDetails = details;
                vehicle1.Model = model;
                vehicle1.Model.Make = make;
                vehicle1.Thumbnail = image;
                return vehicle1;
            })
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
    
    [Benchmark]
    public async Task<VehiclesResult> GetSimpleVehicles_Mid_Dapper()
    {
        using var connection = await ConnectionFactory.Create(DataSchemaConstants.ConnectionString);

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
            new { Skip = (MidPage - 1) * MidPageSize, Take = MidPageSize });

        var vehicles = multiRead.Read<Vehicle>().ToList();
        var totalCount = multiRead.Read<int>().First();

        return new VehiclesResult(vehicles, totalCount);
    }

    [Benchmark]
    public async Task<VehiclesResult> GetCompleteVehicles_Mid_Dapper()
    {
        using var connection = await ConnectionFactory.Create(DataSchemaConstants.ConnectionString);

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
            
            DECLARE @VehicleIds TABLE (Id INT);
            INSERT INTO @VehicleIds
            SELECT Id FROM DapperVsEfCore.Vehicles ORDER BY Id OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY
            
            SELECT image.*
            FROM DapperVsEfCore.Images image
            WHERE image.VehicleId IN (select Id from @VehicleIds)
            
            SELECT damageImage.*
            FROM DapperVsEfCore.DamageImage damageImage
            WHERE damageImage.VehicleId IN (select Id from @VehicleIds)
            
            SELECT ov.VehicleId, o.*
            FROM DapperVsEfCore.Options o
                     INNER JOIN DapperVsEfCore.OptionVehicle ov ON o.Id = ov.OptionsId
            WHERE ov.VehicleId IN (select Id from @VehicleIds)
            
            SELECT tv.VehicleId, t.*
            FROM DapperVsEfCore.Tags t
                     INNER JOIN DapperVsEfCore.TagVehicle tv ON t.Id = tv.TagsId
            WHERE tv.VehicleId IN (select Id from @VehicleIds)
            
            SELECT COUNT(*)
            FROM DapperVsEfCore.Vehicles
            """;

        await using var multiRead = await connection.QueryMultipleAsync(
            sql,
            new { Skip = (MidPage - 1) * MidPageSize, Take = MidPageSize });

        var vehiclesDictionary = multiRead
            .Read<Vehicle, EngineDetails, Model, Make, Image, Vehicle>((vehicle1, details, model, make, image) =>
            {
                vehicle1.EngineDetails = details;
                vehicle1.Model = model;
                vehicle1.Model.Make = make;
                vehicle1.Thumbnail = image;
                return vehicle1;
            })
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

    [Benchmark]
    public async Task<VehiclesResult> GetSimpleVehicles_Dapper()
    {
        using var connection = await ConnectionFactory.Create(DataSchemaConstants.ConnectionString);

        const string sql =
            """
            SELECT v.*
            FROM DapperVsEfCore.Vehicles v

            SELECT COUNT(*)
            FROM DapperVsEfCore.Vehicles
            """;

        await using var multiRead = await connection.QueryMultipleAsync(sql);

        var vehicles = multiRead.Read<Vehicle>().ToList();
        var totalCount = multiRead.Read<int>().First();

        return new VehiclesResult(vehicles, totalCount);       
    }

    [Benchmark]
    public async Task<VehiclesResult> GetCompleteVehicles_Dapper()
    {
        using var connection = await ConnectionFactory.Create(DataSchemaConstants.ConnectionString);

        const string sql =
            """
            SELECT v.*, e.*, model.*, make.*, image.*
            FROM DapperVsEfCore.Vehicles v
                     INNER JOIN DapperVsEfCore.EngineDetails e ON v.EngineDetailsId = e.Id
                     INNER JOIN DapperVsEfCore.Models model ON model.Id = v.ModelId
                     INNER JOIN DapperVsEfCore.Makes make ON make.Id = model.MakeId
                     INNER JOIN DapperVsEfCore.Images image ON image.Id = v.ThumbnailId
            
            DECLARE @VehicleIds TABLE (Id INT);
            INSERT INTO @VehicleIds
            SELECT Id FROM DapperVsEfCore.Vehicles 
            
            SELECT image.*
            FROM DapperVsEfCore.Images image
            WHERE image.VehicleId IN (select Id from @VehicleIds)
            
            SELECT damageImage.*
            FROM DapperVsEfCore.DamageImage damageImage
            WHERE damageImage.VehicleId IN (select Id from @VehicleIds)
            
            SELECT ov.VehicleId, o.*
            FROM DapperVsEfCore.Options o
                     INNER JOIN DapperVsEfCore.OptionVehicle ov ON o.Id = ov.OptionsId
            WHERE ov.VehicleId IN (select Id from @VehicleIds)
            
            SELECT tv.VehicleId, t.*
            FROM DapperVsEfCore.Tags t
                     INNER JOIN DapperVsEfCore.TagVehicle tv ON t.Id = tv.TagsId
            WHERE tv.VehicleId IN (select Id from @VehicleIds)
            
            SELECT COUNT(*)
            FROM DapperVsEfCore.Vehicles
            """;

        await using var multiRead = await connection.QueryMultipleAsync(sql);

        var vehiclesDictionary = multiRead
            .Read<Vehicle, EngineDetails, Model, Make, Image, Vehicle>((vehicle1, details, model, make, image) =>
            {
                vehicle1.EngineDetails = details;
                vehicle1.Model = model;
                vehicle1.Model.Make = make;
                vehicle1.Thumbnail = image;
                return vehicle1;
            })
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
}