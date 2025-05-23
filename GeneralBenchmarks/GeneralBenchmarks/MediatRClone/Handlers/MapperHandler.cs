namespace GeneralBenchmarks.MediatRClone.Handlers;

public class MapperHandler<TRequest, TResponse>(IMapper<TRequest> mapper, IRequestHandler<TRequest, TResponse> inner) 
    : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IMapper<TRequest> _mapper = mapper;
    private readonly IRequestHandler<TRequest, TResponse> _inner = inner;

    public async Task<TResponse> HandleAsync(TRequest request)
    {
        var alteredRequest = _mapper.Map(request);
        return await _inner.HandleAsync(alteredRequest);
    }
}