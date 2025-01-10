using System.Text.Json;

namespace GeneralBenchmarks.Mapping;

public static class JsonMappingExtensions
{
    public static TDestination? MapTo<TDestination>(this object source)
    {
        var serializedObject = JsonSerializer.Serialize(source);
        return JsonSerializer.Deserialize<TDestination>(serializedObject);
    }
}