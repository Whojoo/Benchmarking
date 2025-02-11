using BenchmarkDotNet.Attributes;
using Bogus;
using FluentValidation;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace GeneralBenchmarks.FluentValidationBenchmark;

[MemoryDiagnoser]
[VeryLongRunJob]
public class FluentValidationBenchmark
{
    private Model _model = null!;

    private readonly Faker<Model> _modelFactory = new Faker<Model>()
        .RuleFor(model => model.FullName, faker => faker.Person.FirstName.OrNull(faker))
        .RuleFor(model => model.DateOfBirth, faker => DateOnly.FromDateTime(faker.Person.DateOfBirth))
        .RuleFor(model => model.Email, faker => faker.Person.Email.OrNull(faker))
        .RuleFor(model => model.PhoneNumber, faker => faker.Person.Phone.OrNull(faker))
        .RuleFor(model => model.Gender, faker => (Gender)faker.Random.Int(-5, 5));

    private readonly IValidator<Model> _fluentValidator = new FluentValidator();
    private readonly ICustomValidator<Model> _customValidator = new CustomValidator();

    [IterationSetup]
    public void Setup()
    {
        _model = _modelFactory.Generate();
    }

    [Benchmark]
    public ValidationResult FluentValidation()
    {
        return _fluentValidator.Validate(_model);
    }

    [Benchmark]
    public ValidationResult CustomValidation()
    {
        return _customValidator.Validate(_model);
    }
}