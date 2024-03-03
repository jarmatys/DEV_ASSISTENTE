namespace ASSISTENTE.Common;

public sealed class Result<T>(bool isSuccess, Error? errorMessage)
{
    public bool IsSuccess { get; } = isSuccess;
    public bool IsFailure { get; } = !isSuccess;
    
    public Error? ErrorMessage { get; } = errorMessage;

    public T? Value { get; private init; }
    
    public static Result<T> Ok() => new(isSuccess: true, errorMessage: null);
    public static Result<T> Fail(Error message) => new(isSuccess: false, errorMessage: message);
    public static Result<T> Ok(T value) => new(isSuccess: true, errorMessage: null) { Value = value };
}