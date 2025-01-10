using Bogus;

namespace GeneralBenchmarks.Mapping;

public static class ObjectToMapFactory
{
    private static readonly Faker<ObjectToMap> Faker = new Faker<ObjectToMap>()
        .RuleFor(o => o.Id, f => f.Random.Guid())
        .RuleFor(o => o.FirstName, f => f.Person.FirstName)
        .RuleFor(o => o.LastName, f => f.Person.LastName)
        .RuleFor(o => o.Gender, f => f.Person.Gender.ToString())
        .RuleFor(o => o.BirthDate, f => DateOnly.FromDateTime(f.Person.DateOfBirth));

    public static List<ObjectToMap> GenerateList(int count)
    {
        return Enumerable.Range(0, count)
            .Select(_ => Faker.Generate())
            .ToList();
    }

    public static ObjectToMap GenerateSingle() => Faker.Generate();
}