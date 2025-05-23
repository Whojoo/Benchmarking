namespace GeneralBenchmarks.MediatRClone;

public interface IValidator<in T>
{
    bool Validate(T value);
}