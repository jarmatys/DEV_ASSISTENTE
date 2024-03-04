namespace ASSISTENTE.Common.Extensions;

public static class ResultExtensions
{
    public static Result<T> ToResult<T>(this T value)
    {
        return Result<T>.Ok(value);
    }

    public static Result<T> ToResult<T>(this T value, Error error)
    {
        return Result<T>.Fail(error);
    }

    public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
    {
        if (result is { IsSuccess: true, Value: not null })
        {
            action(result.Value);
        }

        return result;
    }

    public static Result<TNew> OnSuccess<T, TNew>(this Result<T> result, Func<T, Result<TNew>> func)
    {
        if (result.IsSuccess && result.Value != null)
        {
            return func(result.Value);
        }

        if (result is { IsFailure: true, ErrorMessage: not null })
        {
            return Result<TNew>.Fail(result.ErrorMessage);
        }

        throw new InvalidOperationException("Result is not in a valid state");
    }

    public static Result<T> OnFailure<T>(this Result<T> result, Action<Error> action)
    {
        if (result is { IsFailure: true, ErrorMessage: not null })
        {
            action(result.ErrorMessage);
        }

        return result;
    }

    public static Result<T> OnBoth<T>(this Result<T> result, Action<Result<T>> action)
    {
        action(result);

        return result;
    }
}