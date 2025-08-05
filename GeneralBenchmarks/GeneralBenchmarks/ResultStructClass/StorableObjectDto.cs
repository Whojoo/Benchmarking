namespace GeneralBenchmarks.ResultStructClass;

public class StorableObjectDto
{
    public Guid Id { get; set; }
    public StorableRequiredObjectDto StorableRequired { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTimeOffset CreationTime { get; set; }
}

public record StorableRequiredObjectDto(Guid Id, string Name);

public static class StorableObjectDtoExtensions
{
    public static StorableObjectDto ToDto(this StorableObject storableObject)
    {
        return new StorableObjectDto
        {
            Id = storableObject.Id,
            StorableRequired = new StorableRequiredObjectDto(storableObject.StorableRequired!.Id, storableObject.StorableRequired!.Name),
            Name = storableObject.Name,
            CreationTime = storableObject.CreationTime
        };
    }
}