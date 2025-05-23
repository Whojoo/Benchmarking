namespace GeneralBenchmarks.MediatRClone;

public interface IMapper<T>
{
    T Map(T value);
}