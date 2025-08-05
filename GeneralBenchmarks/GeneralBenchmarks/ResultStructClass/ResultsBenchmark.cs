using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;

namespace GeneralBenchmarks.ResultStructClass;

[MemoryDiagnoser]
[VeryLongRunJob]
public class ResultsBenchmark
{
    private Guid _requiredId = Guid.CreateVersion7();
    private ResultDbContext _dbContext = null!;

    [IterationSetup]
    public void IterationSetup()
    {
        _dbContext = new ResultDbContext();
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();

        _dbContext.StorableRequiredObjects.Add(new StorableRequiredObject
        {
            Id = _requiredId,
            Name = "Required"
        });
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        _dbContext.Dispose();
    }

    [Benchmark]
    public async Task<bool> ClassSuccess()
    {
        const string name = "Test";
        return await PerformForClass(_dbContext, name, _requiredId);
    }

    [Benchmark]
    public async Task<bool> StructSuccess()
    {
        const string name = "Test";
        return await PerformForStruct(_dbContext, name, _requiredId);
    }
    
    [Benchmark]
    public async Task<bool> ClassEmptyName()
    {
        const string name = "";
        return await PerformForClass(_dbContext, name, _requiredId);
    }
    
    [Benchmark]
    public async Task<bool> StructEmptyName()
    {
        const string name = "";
        return await PerformForStruct(_dbContext, name, _requiredId);
    }

    [Benchmark]
    public async Task<bool> ClassRequiredObjectMissing()
    {
        const string name = "Test";
        return await PerformForClass(_dbContext, name, Guid.Empty);
    }
    
    [Benchmark]
    public async Task<bool> StructRequiredObjectMissing()
    {
        const string name = "Test";
        return await PerformForStruct(_dbContext, name, Guid.Empty);   
    }

    private static async Task<bool> PerformForClass(ResultDbContext dbContext, string? name, Guid requiredId)
    {
        return await GetRequiredObjectForClassAsync(dbContext, requiredId)
            .ErrorIfAsync(_ => string.IsNullOrWhiteSpace(name), ResultErrors.NameIsRequiredError)
            .BindAsync(async requiredObject =>
            {
                var storableObject = new StorableObject
                {
                    Name = name!,
                    CreationTime = DateTimeOffset.UtcNow,
                    StorableRequired = requiredObject
                };
                dbContext.StorableObjects.Add(storableObject);
                var storeResult = await dbContext.SaveChangesAsync();
                return storeResult > 0 
                    ? ResultClass<StorableObject>.Success(storableObject) 
                    : ResultClass<StorableObject>.Failure(ResultErrors.StoreError);
            })
            .MapAsync(x => x.ToDto())
            .MatchAsync(_ => true, _ => false);
    }
    
    private static async Task<ResultClass<StorableRequiredObject>> GetRequiredObjectForClassAsync(ResultDbContext dbContext, Guid id)
    {
        var obj = await dbContext.StorableRequiredObjects
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
        return obj is null 
            ? ResultClass<StorableRequiredObject>.Failure(ResultErrors.RequiredObjectMissingError) 
            : ResultClass<StorableRequiredObject>.Success(obj);
    }
    
    private static async Task<bool> PerformForStruct(ResultDbContext dbContext, string? name, Guid requiredId)
    {
        return await GetRequiredObjectForStructAsync(dbContext, requiredId)
            .ErrorIfAsync(_ => string.IsNullOrWhiteSpace(name), ResultErrors.NameIsRequiredErrorStruct)
            .BindAsync(async requiredObject =>
            {
                var storableObject = new StorableObject
                {
                    Name = name!,
                    CreationTime = DateTimeOffset.UtcNow,
                    StorableRequired = requiredObject
                };
                dbContext.StorableObjects.Add(storableObject);
                var storeResult = await dbContext.SaveChangesAsync();
                return storeResult > 0 
                    ? ResultStruct<StorableObject>.Success(storableObject) 
                    : ResultStruct<StorableObject>.Failure(ResultErrors.StoreErrorStruct);
            })
            .MapAsync(x => x.ToDto())
            .MatchAsync(_ => true, _ => false);
    }
    
    private static async Task<ResultStruct<StorableRequiredObject>> GetRequiredObjectForStructAsync(ResultDbContext dbContext, Guid id)
    {
        var obj = await dbContext.StorableRequiredObjects
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
        return obj is null 
            ? ResultStruct<StorableRequiredObject>.Failure(ResultErrors.RequiredObjectMissingErrorStruct) 
            : ResultStruct<StorableRequiredObject>.Success(obj);
    }

}

public static class ResultErrors
{
    public static readonly ResultErrorClass RequiredObjectMissingError = new("RequiredObjectMissingError", "Required object is missing");
    public static readonly ResultErrorClass NameIsRequiredError = new("NameIsRequiredError", "Name is required");
    public static readonly ResultErrorClass StoreError = new("StoreError", "Error storing object");
    
    public static readonly ResultErrorStruct RequiredObjectMissingErrorStruct = new("RequiredObjectMissingError", "Required object is missing");
    public static readonly ResultErrorStruct NameIsRequiredErrorStruct = new("NameIsRequiredError", "Name is required");
    public static readonly ResultErrorStruct StoreErrorStruct = new("StoreError", "Error storing object");
}