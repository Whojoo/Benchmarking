namespace GeneralBenchmarks.MediatRClone;

public interface IRequestPipeline<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> ExecuteAsync(TRequest request, Func<TRequest, Task<TResponse>> next);
}