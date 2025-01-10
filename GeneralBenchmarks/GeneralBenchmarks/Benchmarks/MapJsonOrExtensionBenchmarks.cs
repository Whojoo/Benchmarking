using AutoMapper;
using BenchmarkDotNet.Attributes;
using GeneralBenchmarks.Mapping;
using Mapster;

namespace GeneralBenchmarks.Benchmarks;

[MemoryDiagnoser]
[SimpleJob]
public class MapJsonOrExtensionBenchmarks
{
    private List<ObjectToMap> _objectsToMap = [];
    private ObjectToMap _objectToMap = null!;

    private MapperlyMapper _mapperlyMapper = null!;
    private IMapper _automapper = null!;

    [GlobalSetup]
    public void SetupMappers()
    {
        _mapperlyMapper = new MapperlyMapper();

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ObjectToMap, MappedObject>();
        });
        _automapper = configuration.CreateMapper();
    }
    
    [IterationSetup]
    public void IterationSetup()
    {
        _objectsToMap = ObjectToMapFactory.GenerateList(10000);
        _objectToMap = ObjectToMapFactory.GenerateSingle();
    }

    [Benchmark]
    public MappedObject MapSingleByExtension()
    {
        return _objectToMap.ToMappedObject();
    }

    [Benchmark]
    public MappedObject MapSingleByStaticMapper()
    {
        return ObjectToMapMapper.MapToMappedObject(_objectToMap);
    }

    [Benchmark]
    public MappedObject MapSingleByJson()
    {
        return _objectToMap.MapTo<MappedObject>();
    }

    [Benchmark]
    public MappedObject MapSingleByMapperly()
    {
        return _mapperlyMapper.ObjectToMapToMappedObject(_objectToMap);
    }

    [Benchmark]
    public MappedObject MapSingleByMapster()
    {
        return _objectToMap.Adapt<MappedObject>();
    }
    
    [Benchmark]
    public MappedObject MapSingleByAutomapper()
    {
        return _automapper.Map<MappedObject>(_objectToMap);
    }

    [Benchmark]
    public List<MappedObject> MapListByExtension()
    {
        return _objectsToMap.ToMappedObjectsSelectToList();
    }

    [Benchmark]
    public List<MappedObject> MapListByExtension2()
    {
        return _objectsToMap.ToMappedObjectsPreCreateList();
    }

    [Benchmark]
    public List<MappedObject> MapListByStaticMapper()
    {
        return ObjectToMapMapper.MapToMappedObjects(_objectsToMap);
    }
    
    [Benchmark]
    public List<MappedObject> MapListByJson()
    {
        return _objectsToMap.MapTo<List<MappedObject>>();
    }

    [Benchmark]
    public List<MappedObject> MapListByMapperly()
    {
        return _mapperlyMapper.ObjectsToMapToMappedObjects(_objectsToMap);
    }

    [Benchmark]
    public List<MappedObject> MapListByMapster()
    {
        return _objectsToMap.Adapt<List<MappedObject>>();
    }

    [Benchmark]
    public List<MappedObject> MapListByAutomapper()
    {
        return _automapper.Map<List<MappedObject>>(_objectsToMap);
    }
}