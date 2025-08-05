namespace GeneralBenchmarks.ResultStructClass;

public class StorableObject
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid StorableRequiredId { get; set; }
    public StorableRequiredObject? StorableRequired { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset CreationTime { get; set; }
}

public class StorableRequiredObject
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = string.Empty;
}