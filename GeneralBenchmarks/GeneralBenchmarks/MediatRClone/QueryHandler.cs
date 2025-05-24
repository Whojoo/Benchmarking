using GeneralBenchmarks.MediatRClone.Dependencies;

namespace GeneralBenchmarks.MediatRClone;

public abstract class QueryHandler<TRequest, TResponse>(
    MockLogger logger,
    IValidator<TRequest>? validator,
    IMapper<TRequest> mapper)
    : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly MockLogger _logger = logger;
    private readonly IValidator<TRequest>? _validator = validator;
    private readonly IMapper<TRequest> _mapper = mapper;

    public async Task<TResponse> HandleAsync(TRequest request)
    {
        _logger.Log("Handling request");
        
        var alteredRequest = _mapper.Map(request);
        
        if (_validator is not null && !_validator.Validate(alteredRequest))
            return default!;
        
        var result = await ExecuteHandlerAsync(alteredRequest);
        
        _logger.Log("Handled request");

        return result;
    }
    
    protected abstract Task<TResponse> ExecuteHandlerAsync(TRequest request);
}