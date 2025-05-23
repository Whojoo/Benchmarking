namespace GeneralBenchmarks.MediatRClone.Handlers;

public class ValidatorRequestHandler<TRequest, TResponse>(IValidator<TRequest> validator, IRequestHandler<TRequest, TResponse> inner)
    : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest> _validator = validator;
    private readonly IRequestHandler<TRequest, TResponse> _inner = inner;

    public async Task<TResponse> HandleAsync(TRequest request)
    {
        if (_validator.Validate(request))
            return await _inner.HandleAsync(request);

        return default!;
    }
}