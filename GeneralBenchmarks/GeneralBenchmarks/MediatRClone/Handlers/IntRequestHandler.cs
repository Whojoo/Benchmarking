namespace GeneralBenchmarks.MediatRClone.Handlers;

public class IntRequestHandler
    : IRequestHandler<IntRequest, int>
{
    public Task<int> HandleAsync(IntRequest request)
    {
        return Task.FromResult(request.Initial / 2);
    }
}