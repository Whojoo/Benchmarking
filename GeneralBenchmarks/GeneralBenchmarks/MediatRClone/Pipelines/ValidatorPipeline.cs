namespace GeneralBenchmarks.MediatRClone.Pipelines;

public class ValidatorPipeline<TRequest, TResponse>(IValidator<TRequest> validator)
    : IRequestPipeline<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest> _validator = validator;

    public async Task<TResponse> ExecuteAsync(TRequest request, Func<TRequest, Task<TResponse>> next)
    {
        if (_validator.Validate(request))
            return await next.Invoke(request);
        
        return default!;
    }
}