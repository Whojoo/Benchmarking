using GeneralBenchmarks.MediatRClone.Dependencies;

namespace GeneralBenchmarks.MediatRClone.Handlers;

public class TemplateIntRequestHandler(MockLogger logger, IValidator<IntRequest>? validator, IMapper<IntRequest> mapper) 
    : QueryHandler<IntRequest, int>(logger, validator, mapper)
{
    protected override Task<int> ExecuteHandlerAsync(IntRequest request)
    {
        return Task.FromResult(request.Initial / 2);
    }
}