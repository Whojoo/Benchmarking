
using Riok.Mapperly.Abstractions;

namespace GeneralBenchmarks.Mapping;

[Mapper]
public partial class MapperlyMapper
{
    public partial MappedObject ObjectToMapToMappedObject(ObjectToMap objectToMap);
    public partial List<MappedObject> ObjectsToMapToMappedObjects(List<ObjectToMap> objectsToMap);
}