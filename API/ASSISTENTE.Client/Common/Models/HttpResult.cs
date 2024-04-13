namespace ASSISTENTE.Client.Common.Models;

public sealed class HttpResult<TResponse>
{
    private HttpResult(TResponse content)
    {
        Content = content;
        IsSuccess = true;
    }

    private HttpResult(ErrorResponse errorDetails)
    {
        ErrorDetails = errorDetails;
        IsSuccess = false;
    }
    
    public TResponse? Content { get; }
    public ErrorResponse? ErrorDetails { get; }
    
    public bool IsSuccess { get; }
    
    public bool IsFailure => !IsSuccess;
    
    public static HttpResult<TResponse> Success(TResponse content) => new(content);
    
    public static HttpResult<TResponse> Failure(ErrorResponse errorDetails) => new(errorDetails);
}