namespace GeneralBenchmarks.ResultStructClass;

public record ResultErrorClass(string Code, string Description);

public class ResultClass<T>
{
    private readonly T? _value;
    private readonly List<ResultErrorClass> _errors = [];

    public bool IsSuccess => _value is not null && _errors.Count == 0;
    
    public ResultClass(List<ResultErrorClass> errors) => _errors = errors;
    public ResultClass(T value) => _value = value;

    public TMatchValue Match<TMatchValue>(Func<T, TMatchValue> successAction, Func<List<ResultErrorClass>, TMatchValue> failureAction)
    {
        return IsSuccess ? successAction(_value!) : failureAction(_errors);
    }

    public async Task<ResultClass<TBindValue>> BindAsync<TBindValue>(Func<T, Task<ResultClass<TBindValue>>> bindFunc)
    {
        return IsSuccess ? await bindFunc(_value!) : new ResultClass<TBindValue>(_errors);
    }

    public ResultClass<TMapValue> Map<TMapValue>(Func<T, TMapValue> mapFunc)
    {
        return IsSuccess ? new ResultClass<TMapValue>(mapFunc(_value!)) : new ResultClass<TMapValue>(_errors);
    }

    public ResultClass<T> ErrorIf(Func<T, bool> predicate, ResultErrorClass errorClass)
    {
        if (!IsSuccess)
            return this;
        
        return predicate(_value!) ? Failure(errorClass) : this;
    }
    
    public static ResultClass<T> Success(T value) => new(value);
    public static ResultClass<T> Failure(string error) => new([new ResultErrorClass("General.Error", error)]);
    public static ResultClass<T> Failure(ResultErrorClass errorClass) => new([errorClass]);
    public static ResultClass<T> Failure(List<ResultErrorClass> errors) => new(errors);
}

public static class ResultAsyncMatchExtensions
{
    public static async Task<TMatchValue> MatchAsync<TSource, TMatchValue>(
        this Task<ResultClass<TSource>> result,
        Func<TSource, TMatchValue> successAction,
        Func<List<ResultErrorClass>, TMatchValue> failureAction)
    {
        return (await result).Match(successAction, failureAction);
    }

    public static async Task<ResultClass<TBindValue>> BindAsync<TSource, TBindValue>(
        this Task<ResultClass<TSource>> result,
        Func<TSource, Task<ResultClass<TBindValue>>> bindFunc)
    {
        return await (await result).BindAsync(bindFunc);
    }

    public static async Task<ResultClass<TMapValue>> MapAsync<TSource, TMapValue>(
        this Task<ResultClass<TSource>> result,
        Func<TSource, TMapValue> mapFunc)
    {
        return (await result).Map(mapFunc);
    }

    public static async Task<ResultClass<T>> ErrorIfAsync<T>(
        this Task<ResultClass<T>> result,
        Func<T, bool> predicate,
        ResultErrorClass errorClass)
    {
        return (await result).ErrorIf(predicate, errorClass);
    }
}
