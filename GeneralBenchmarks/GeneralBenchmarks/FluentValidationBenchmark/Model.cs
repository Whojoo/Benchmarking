namespace GeneralBenchmarks.FluentValidationBenchmark;

public class Model
{
    public string? FullName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public Gender Gender { get; set; }
}

public enum Gender
{
    Male, Female, Other
}