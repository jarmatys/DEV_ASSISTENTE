namespace ASSISTENTE.UI.Brokers.Models;

public class HttpResult
{
    protected HttpResult()
    {
        IsSuccess = true;
    }

    protected HttpResult(ErrorResponse errorDetails)
    {
        ErrorDetails = errorDetails;
        IsSuccess = false;
    }

    public ErrorResponse? ErrorDetails { get; }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public static HttpResult Success() => new();

    public static HttpResult Failure(ErrorResponse errorResponse) => new(errorResponse);

    public static HttpResult Failure(int statusCode, string errorCode, string errorMessage) 
        => new(CreateErrorResponse(statusCode, errorCode, errorMessage));

    protected static ErrorResponse CreateErrorResponse(int statusCode, string errorCode, string errorMessage)
        => new()
        {
            StatusCode = statusCode,
            Message = errorMessage,
            Errors = new Dictionary<string, List<string>>()
            {
                { errorCode, [errorMessage] }
            }
        };
}

public sealed class HttpResult<TResponse> : HttpResult
{
    private HttpResult(TResponse content)
    {
        Content = content;
    }

    private HttpResult(ErrorResponse errorDetails) : base(errorDetails)
    {
    }

    public TResponse? Content { get; }

    public static HttpResult<TResponse> Success(TResponse content) => new(content);

    public new static HttpResult<TResponse> Failure(int statusCode, string errorCode, string errorMessage)
        => new(CreateErrorResponse(statusCode, errorCode, errorMessage));

    public new static HttpResult<TResponse> Failure(ErrorResponse errorDetails) => new(errorDetails);
}