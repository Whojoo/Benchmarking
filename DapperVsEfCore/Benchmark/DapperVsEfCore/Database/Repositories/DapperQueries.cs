namespace Benchy.DapperVsEfCore.Database.Repositories;

public static class DapperQueries
{
    public const string GetSimpleById = """
                                        SELECT v.*
                                        FROM DapperVsEfCore.Vehicles v
                                        WHERE v.Id = @VehicleId
                                        """;

    public const string GetCompleteById = """
                                          SELECT v.*, e.*, model.*, make.*, image.*
                                          FROM DapperVsEfCore.Vehicles v
                                          JOIN DapperVsEfCore.EngineDetails e ON v.EngineDetailsId = e.Id
                                          JOIN DapperVsEfCore.Models model ON model.Id = v.ModelId
                                          JOIN DapperVsEfCore.Makes make ON make.Id = model.MakeId
                                          JOIN DapperVsEfCore.Images image ON image.Id = v.ThumbnailId
                                          WHERE v.Id = @VehicleId

                                          SELECT o.*
                                          FROM DapperVsEfCore.Options o
                                          JOIN DapperVsEfCore.OptionVehicle ov ON o.Id = ov.OptionsId
                                          WHERE ov.VehicleId = @VehicleId

                                          SELECT t.*
                                          FROM DapperVsEfCore.Tags t
                                          JOIN DapperVsEfCore.TagVehicle tv ON t.Id = tv.TagsId
                                          WHERE tv.VehicleId = @VehicleId

                                          SELECT i.*
                                          FROM DapperVsEfCore.Images i
                                          WHERE i.VehicleId = @VehicleId

                                          SELECT di.*
                                          FROM DapperVsEfCore.DamageImage di
                                          WHERE di.VehicleId = @VehicleId
                                          """;

    public const string GetSimpleVehicles = """
                                            SELECT v.*
                                            FROM DapperVsEfCore.Vehicles v
                                            ORDER BY v.Id
                                            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY
                                            
                                            SELECT COUNT(*)
                                            FROM DapperVsEfCore.Vehicles
                                            """;

    public const string GetCompleteVehicles = """
                                              SELECT v.*, e.*, model.*, make.*, image.*
                                              FROM DapperVsEfCore.Vehicles v
                                              JOIN DapperVsEfCore.EngineDetails e ON v.EngineDetailsId = e.Id
                                              JOIN DapperVsEfCore.Models model ON model.Id = v.ModelId
                                              JOIN DapperVsEfCore.Makes make ON make.Id = model.MakeId
                                              JOIN DapperVsEfCore.Images image ON image.Id = v.ThumbnailId
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
                                              JOIN DapperVsEfCore.OptionVehicle ov ON o.Id = ov.OptionsId
                                              WHERE ov.VehicleId IN (SELECT Id
                                                                     FROM DapperVsEfCore.Vehicles
                                                                     ORDER BY Id
                                                                     OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY)   
                                                                     
                                              SELECT tv.VehicleId, t.*
                                              FROM DapperVsEfCore.Tags t
                                              JOIN DapperVsEfCore.TagVehicle tv ON t.Id = tv.TagsId
                                              WHERE tv.VehicleId IN (SELECT Id
                                                                     FROM DapperVsEfCore.Vehicles
                                                                     ORDER BY Id
                                                                     OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY)   
                                                                     
                                              SELECT COUNT(*)
                                              FROM DapperVsEfCore.Vehicles
                                              """;
}