namespace GeneralBenchmarks.Mapping;

public class ObjectToMap
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Gender { get; set; }
}