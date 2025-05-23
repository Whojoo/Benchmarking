namespace GeneralBenchmarks.MediatRClone.Dependencies;

public class MockMapper : IMapper<IntRequest>
{
    public IntRequest Map(IntRequest value)
    {
        return new IntRequest(Math.Abs(value.Initial * 2));
    }
}