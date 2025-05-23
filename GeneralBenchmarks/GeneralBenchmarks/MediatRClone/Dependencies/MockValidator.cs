namespace GeneralBenchmarks.MediatRClone.Dependencies;

public class MockValidator : IValidator<IntRequest>
{
    public bool Validate(IntRequest value) => value.Initial > 0;
}