namespace GeneralBenchmarks.MediatRClone;

public class RequestExecutor<TRequest, TResponse>(
    IEnumerable<IRequestPipeline<TRequest, TResponse>> pipelines,
    IRequestHandler<TRequest, TResponse> handler)
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IRequestPipeline<TRequest, TResponse>> _pipelines = pipelines;
    private readonly IRequestHandler<TRequest, TResponse> _handler = handler;

    public Task<TResponse> ExecuteAsync(TRequest request) =>
        _pipelines
            .Aggregate<IRequestPipeline<TRequest, TResponse>, Func<TRequest, Task<TResponse>>>(
                passedRequest => _handler.HandleAsync(passedRequest),
                (next, pipeline) => passedRequest => pipeline.ExecuteAsync(passedRequest, next))
            (request);
}