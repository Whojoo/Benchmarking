using Benchy.DapperVsEfCore.Models;
using Bogus;

namespace Benchy.DapperVsEfCore.Database.Factories;

public static class DataFactory
{
    private const int Makes = 10;
    private const int AverageModelsPerMake = 5;
    private const int Models = Makes * AverageModelsPerMake;
    private const int Tags = 20;
    private const int Options = 25;
    private const int Vehicles = 250;
    private const int AverageImagesPerVehicle = 15;
    private const int AverageDamageImagesPerVehicle = 5;

    public static List<Vehicle> GenerateVehicles()
    {
        Randomizer.Seed = new Random(1337);
        var options = GenerateOptions();
        var models = GenerateModels();
        var tags = GenerateTags();

        var engineDetailsFaker = new Faker<EngineDetails>()
            .RuleFor(engineDetails => engineDetails.Pk, faker => faker.Random.Int(90, 1000))
            .RuleFor(engineDetails => engineDetails.MaxSpeed, faker => faker.Random.Int(160, 240))
            .RuleFor(engineDetails => engineDetails.NumberOfCilinders, faker => faker.Random.Int(2, 5));

        var imageFaker = new Faker<Image>()
            .RuleFor(image => image.SmallUrl, faker => faker.Image.PlaceImgUrl())
            .RuleFor(image => image.MediumUrl, faker => faker.Image.PlaceImgUrl())
            .RuleFor(image => image.OriginalUrl, faker => faker.Image.PlaceImgUrl());

        var damageImageFaker = new Faker<DamageImage>()
            .RuleFor(image => image.SmallUrl, faker => faker.Image.PlaceImgUrl())
            .RuleFor(image => image.MediumUrl, faker => faker.Image.PlaceImgUrl())
            .RuleFor(image => image.OriginalUrl, faker => faker.Image.PlaceImgUrl());

        var vehicleFaker = new Faker<Vehicle>()
            .RuleFor(vehicle => vehicle.EngineDetails, _ => engineDetailsFaker.Generate())
            .RuleFor(vehicle => vehicle.Price, faker => faker.Random.Decimal(5_000, 50_000))
            .RuleFor(vehicle => vehicle.LicensePlate, faker => faker.Vehicle.Vin())
            .RuleFor(vehicle => vehicle.Thumbnail, _ => imageFaker.Generate())
            .RuleFor(vehicle => vehicle.Options, faker => PickMultipleRandom(faker, options, faker.Random.Int(1, Options - 10)))
            .RuleFor(vehicle => vehicle.Tags, faker => PickMultipleRandom(faker, tags, faker.Random.Int(1, Tags - 10)))
            .RuleFor(vehicle => vehicle.Model, faker => faker.PickRandom(models))
            .RuleFor(vehicle => vehicle.DamageImages, faker => damageImageFaker.Generate(faker.Random.Int(0, AverageDamageImagesPerVehicle * 2)))
            .RuleFor(vehicle => vehicle.DetailImages, faker => imageFaker.Generate(faker.Random.Int(1, AverageImagesPerVehicle * 2)));

        return vehicleFaker.Generate(Vehicles);
    }

    private static List<Tag> PickMultipleRandom(Faker faker, List<Tag> items, int count)
    {
        HashSet<string> idSet = [];
        List<Tag> result = [];

        while (result.Count < count)
        {
            var item = faker.PickRandom(items);

            if (!idSet.Add(item.Code))
            {
                continue;
            }

            result.Add(item);
        }

        return result;
    }
    private static List<Option> PickMultipleRandom(Faker faker, List<Option> items, int count)
    {
        HashSet<string> idSet = [];
        List<Option> result = [];

        while (result.Count < count)
        {
            var item = faker.PickRandom(items);

            if (!idSet.Add(item.Code))
            {
                continue;
            }

            result.Add(item);
        }

        return result;
    }

    private static List<Make> GenerateMakes()
    {
        var makeFaker = new Faker<Make>()
            .RuleFor(make => make.Name, faker => faker.Vehicle.Manufacturer());

        return makeFaker.Generate(Makes);
    }

    private static List<Model> GenerateModels()
    {
        var makes = GenerateMakes();
        var modelFaker = new Faker<Model>()
            .RuleFor(model => model.Name, faker => faker.Vehicle.Model())
            .RuleFor(model => model.Make, faker => faker.PickRandom(makes));

        return modelFaker.Generate(Models);
    }

    private static List<Option> GenerateOptions()
    {
        var optionFaker = new Faker<Option>()
            .RuleFor(option => option.Name, faker => faker.Commerce.ProductName())
            .RuleFor(option => option.Code, faker => faker.Random.Guid().ToString());

        return optionFaker.Generate(Options);
    }

    private static List<Tag> GenerateTags()
    {
        var tagFaker = new Faker<Tag>()
            .RuleFor(tag => tag.Name, faker => faker.Random.String(5, DataSchemaConstants.VarCharLength))
            .RuleFor(tag => tag.Code, faker => faker.Random.Guid().ToString());

        return tagFaker.Generate(Tags);
    }
}