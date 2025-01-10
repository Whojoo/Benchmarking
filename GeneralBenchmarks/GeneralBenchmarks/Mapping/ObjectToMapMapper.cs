namespace GeneralBenchmarks.Mapping;

public static class ObjectToMapMapper
{
    public static MappedObject MapToMappedObject(ObjectToMap objectToMap) =>
        new MappedObject
        {
            Id = objectToMap.Id,
            BirthDate = objectToMap.BirthDate,
            FirstName = objectToMap.FirstName,
            LastName = objectToMap.LastName,
            Gender = objectToMap.Gender
        };

    public static List<MappedObject> MapToMappedObjects(List<ObjectToMap> objectsToMap)
    {
        var list = new List<MappedObject>(objectsToMap.Count);
        foreach (var obj in objectsToMap)
        {
            list.Add(MapToMappedObject(obj));
        }

        return list;
    }
}