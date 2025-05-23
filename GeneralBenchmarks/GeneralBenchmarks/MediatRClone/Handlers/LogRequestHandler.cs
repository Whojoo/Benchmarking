using GeneralBenchmarks.MediatRClone.Dependencies;

namespace GeneralBenchmarks.MediatRClone.Handlers;

public class LogRequestHandler<TRequest, TResponse>(MockLogger logger, IRequestHandler<TRequest, TResponse> inner)
    : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly MockLogger _logger = logger;
    private readonly IRequestHandler<TRequest, TResponse> _inner = inner;

    public async Task<TResponse> HandleAsync(TRequest request)
    {
        _logger.Log("Handling request");
        var result = await _inner.HandleAsync(request);
        _logger.Log("Handled request");
        return result;
    }
}