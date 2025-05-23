using BenchmarkDotNet.Attributes;
using GeneralBenchmarks.MediatRClone.Dependencies;
using GeneralBenchmarks.MediatRClone.Handlers;
using GeneralBenchmarks.MediatRClone.Pipelines;
using Microsoft.Extensions.DependencyInjection;

namespace GeneralBenchmarks.MediatRClone;

[MemoryDiagnoser]
public class MediatRAlternativeBenchmark
{
    private IServiceProvider _handlerProvider = null!;
    private IServiceProvider _pipelineProvider = null!;
    
    [GlobalSetup]
    public void Setup()
    {
        var collection = new ServiceCollection()
            .AddSingleton<MockLogger>()
            .AddSingleton<IValidator<IntRequest>, MockValidator>()
            .AddScoped<IMapper<IntRequest>, MockMapper>();

        collection.Scan(scan => scan.FromAssemblyOf<IntRequestHandler>()
            .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        collection.Decorate(typeof(IRequestHandler<,>), typeof(ValidatorRequestHandler<,>));
        collection.Decorate(typeof(IRequestHandler<,>), typeof(MapperHandler<,>));
        collection.Decorate(typeof(IRequestHandler<,>), typeof(LogRequestHandler<,>));

        _handlerProvider = collection.BuildServiceProvider();

        _pipelineProvider = new ServiceCollection()
            .AddSingleton<MockLogger>()
            .AddSingleton<IValidator<IntRequest>, MockValidator>()
            .AddScoped<IMapper<IntRequest>, MockMapper>()
            .AddScoped<IRequestHandler<IntRequest, int>, IntRequestHandler>()
            .AddSingleton(typeof(IRequestPipeline<,>), typeof(ValidatorPipeline<,>))
            .AddScoped(typeof(IRequestPipeline<,>), typeof(MapperPipeline<,>))
            .AddSingleton(typeof(IRequestPipeline<,>), typeof(LogRequestPipeline<,>))
            .AddScoped(typeof(RequestExecutor<,>))
            .BuildServiceProvider();

        using var handlerScope = _handlerProvider.CreateScope();
        _ = handlerScope.ServiceProvider.GetRequiredService<IRequestHandler<IntRequest, int>>();
        
        using var pipelineScope = _pipelineProvider.CreateScope();
        _ = pipelineScope.ServiceProvider.GetRequiredService<RequestExecutor<IntRequest, int>>();
    }

    [Benchmark]
    public async Task<int> Handlers()
    {
        await using var scope = _handlerProvider.CreateAsyncScope();
        var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<IntRequest, int>>();
        var request = new IntRequest(1);
        return await handler.HandleAsync(request);
    }
    
    [Benchmark(Baseline = true)]
    public async Task<int> Pipelines()
    {
        await using var scope = _pipelineProvider.CreateAsyncScope();
        var executor = scope.ServiceProvider.GetRequiredService<RequestExecutor<IntRequest, int>>();
        var request = new IntRequest(1);
        return await executor.ExecuteAsync(request);
    }
}