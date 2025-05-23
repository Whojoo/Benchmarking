namespace GeneralBenchmarks.MediatRClone.Pipelines;

public class MapperPipeline<TRequest, TResponse>(IMapper<TRequest> mapper)
    : IRequestPipeline<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IMapper<TRequest> _mapper = mapper;

    public async Task<TResponse> ExecuteAsync(TRequest request, Func<TRequest, Task<TResponse>> next)
    {
        var alteredRequest = _mapper.Map(request);
        return await next.Invoke(alteredRequest);
    }
}