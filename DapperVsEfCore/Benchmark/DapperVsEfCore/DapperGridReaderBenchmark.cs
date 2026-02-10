using BenchmarkDotNet.Attributes;
using Benchy.DapperVsEfCore.Database;
using Benchy.DapperVsEfCore.Database.Factories;
using Benchy.DapperVsEfCore.Models;
using Dapper;

namespace Benchy.DapperVsEfCore;

[MemoryDiagnoser]
public class DapperGridReaderBenchmark
{
  private const int MidPage = 12;
  private const int MidPageSize = 10;
  
  [Benchmark]
  public async Task<VehiclesResult> GetCompleteVehicles_ToList()
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
  public async Task<VehiclesResult> GetCompleteVehicles_ToArray()
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

    var images = multiRead.Read<Image>().ToArray();
    var damageImages = multiRead.Read<DamageImage>().ToArray();
    var optionTuples = multiRead
      .Read<long, Option, Tuple<long, Option>>(Tuple.Create)
      .ToArray();
    var tagTuples = multiRead
      .Read<long, Tag, Tuple<long, Tag>>(Tuple.Create)
      .ToArray();
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
  public async Task<VehiclesResult> GetCompleteVehicles_Enumerable()
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

    var images = multiRead.Read<Image>();
    var damageImages = multiRead.Read<DamageImage>();
    var optionTuples = multiRead
      .Read<long, Option, Tuple<long, Option>>(Tuple.Create);
    var tagTuples = multiRead
      .Read<long, Tag, Tuple<long, Tag>>(Tuple.Create);
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