namespace GeneralBenchmarks.MediatRClone;

public class IntRequest(int initial) : IRequest<int>
{
    public int Initial { get; set; } = initial;
}