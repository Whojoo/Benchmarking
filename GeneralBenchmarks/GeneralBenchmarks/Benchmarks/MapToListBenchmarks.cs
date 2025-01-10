using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Bogus;
using GeneralBenchmarks.Mapping;

namespace GeneralBenchmarks.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
public class MapToListBenchmarks
{
    private List<ObjectToMap> _objectsToMap = [];

    [Params(100, 1000, 10000)]
    public int AmountOfObjects { get; set; }

    [IterationSetup]
    public void IterationSetup()
    {
        _objectsToMap = ObjectToMapFactory.GenerateList(AmountOfObjects);
    }

    [Benchmark]
    public List<MappedObject> SelectToList()
    {
        return _objectsToMap.ToMappedObjectsSelectToList();
    }

    [Benchmark]
    public List<MappedObject> PreCreateList()
    {
        return _objectsToMap.ToMappedObjectsPreCreateList();
    }

    [Benchmark]
    public List<MappedObject> AddRange()
    {
        return _objectsToMap.ToMappedObjectsAddRange();
    }
}