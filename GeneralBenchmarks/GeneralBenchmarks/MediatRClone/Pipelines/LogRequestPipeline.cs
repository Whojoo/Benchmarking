using GeneralBenchmarks.MediatRClone.Dependencies;

namespace GeneralBenchmarks.MediatRClone.Pipelines;

public class LogRequestPipeline<TRequest, TResponse>(MockLogger logger)
    : IRequestPipeline<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly MockLogger _logger = logger;

    public async Task<TResponse> ExecuteAsync(TRequest request, Func<TRequest, Task<TResponse>> next)
    {
        _logger.Log("Handling request");
        var result = await next.Invoke(request);
        _logger.Log("Handled request");
        return result;
    }
}