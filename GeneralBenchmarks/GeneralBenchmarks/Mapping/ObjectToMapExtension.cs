namespace GeneralBenchmarks.Mapping;

public static class ObjectToMapExtension
{
    public static MappedObject ToMappedObject(this ObjectToMap objectToMap) => new MappedObject
    {
        Id = objectToMap.Id,
        FirstName = objectToMap.FirstName,
        LastName = objectToMap.LastName,
        BirthDate = objectToMap.BirthDate,
        Gender = objectToMap.Gender
    };

    public static List<MappedObject> ToMappedObjectsSelectToList(this List<ObjectToMap> objectsToMap) =>
        objectsToMap
            .Select(o => o.ToMappedObject())
            .ToList();

    public static List<MappedObject> ToMappedObjectsPreCreateList(this List<ObjectToMap> objectsToMap)
    {
        var mappedObjects = new List<MappedObject>(objectsToMap.Count);
        foreach (var objectToMap in objectsToMap)
        {
            mappedObjects.Add(objectToMap.ToMappedObject());
        }

        return mappedObjects;
    }

    public static List<MappedObject> ToMappedObjectsAddRange(this List<ObjectToMap> objectsToMap)
    {
        var mappedObjects = new List<MappedObject>(objectsToMap.Count);
        mappedObjects.AddRange(objectsToMap.Select(o => o.ToMappedObject()));
        return mappedObjects;
    }
}