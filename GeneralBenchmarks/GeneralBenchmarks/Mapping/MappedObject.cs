namespace GeneralBenchmarks.Mapping;

public class MappedObject
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Gender { get; set; }
}