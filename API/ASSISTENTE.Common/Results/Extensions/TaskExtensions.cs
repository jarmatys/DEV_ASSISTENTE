using CSharpFunctionalExtensions;

namespace ASSISTENTE.Common.Results.Extensions;

public static class TaskExtensions 
{
    public static Result<TU> BindAsync<T, TU>(this Result<T> result, Func<T, Task<Result<TU>>> func)
    {
        if (result.IsFailure)
            return Result.Failure<TU>(result.Error);
        
        return func(result.Value).Result;
    }
}