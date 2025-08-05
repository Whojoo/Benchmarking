namespace GeneralBenchmarks.ResultStructClass;

public readonly record struct ResultErrorStruct(string Code, string Description);

public class ResultStruct<T>
{
    private readonly T? _value;
    private readonly List<ResultErrorStruct> _errors = [];

    public bool IsSuccess => _value is not null && _errors.Count == 0;
    
    public ResultStruct(List<ResultErrorStruct> errors) => _errors = errors;
    public ResultStruct(T value) => _value = value;

    public TMatchValue Match<TMatchValue>(Func<T, TMatchValue> successAction, Func<List<ResultErrorStruct>, TMatchValue> failureAction)
    {
        return IsSuccess ? successAction(_value!) : failureAction(_errors);
    }

    public async Task<ResultStruct<TBindValue>> BindAsync<TBindValue>(Func<T, Task<ResultStruct<TBindValue>>> bindFunc)
    {
        return IsSuccess ? await bindFunc(_value!) : new ResultStruct<TBindValue>(_errors);
    }

    public ResultStruct<TMapValue> Map<TMapValue>(Func<T, TMapValue> mapFunc)
    {
        return IsSuccess ? new ResultStruct<TMapValue>(mapFunc(_value!)) : new ResultStruct<TMapValue>(_errors);
    }
    
    public ResultStruct<T> ErrorIf(Func<T, bool> predicate, ResultErrorStruct errorStruct)
    {
        if (!IsSuccess)
            return this;
        
        return predicate(_value!) ? Failure(errorStruct) : this;
    }
    
    public static ResultStruct<T> Success(T value) => new(value);
    public static ResultStruct<T> Failure(string error) => new([new ResultErrorStruct("General.Error", error)]);
    public static ResultStruct<T> Failure(ResultErrorStruct errorStruct) => new([errorStruct]);
    public static ResultStruct<T> Failure(List<ResultErrorStruct> errors) => new(errors);
}

public static class ResultStructAsyncMatchExtensions
{
    public static async Task<TMatchValue> MatchAsync<TSource, TMatchValue>(
        this Task<ResultStruct<TSource>> result,
        Func<TSource, TMatchValue> successAction,
        Func<List<ResultErrorStruct>, TMatchValue> failureAction)
    {
        return (await result).Match(successAction, failureAction);
    }

    public static async Task<ResultStruct<TBindValue>> BindAsync<TSource, TBindValue>(
        this Task<ResultStruct<TSource>> result,
        Func<TSource, Task<ResultStruct<TBindValue>>> bindFunc)
    {
        return await (await result).BindAsync(bindFunc);
    }

    public static async Task<ResultStruct<TMapValue>> MapAsync<TSource, TMapValue>(
        this Task<ResultStruct<TSource>> result,
        Func<TSource, TMapValue> mapFunc)
    {
        return (await result).Map(mapFunc);
    }
    
    public static async Task<ResultStruct<T>> ErrorIfAsync<T>(
        this Task<ResultStruct<T>> result,
        Func<T, bool> predicate,
        ResultErrorStruct errorStruct)
    {
        return (await result).ErrorIf(predicate, errorStruct);
    }
}

