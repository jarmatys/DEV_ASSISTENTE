namespace ASSISTENTE.UI.Common.Models;

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
    
    public static HttpResult<TResponse> Failure(int statusCode, string errorCode, string errorMessage)
    {
        var errorDetails = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = errorMessage,
            Errors = new Dictionary<string, List<string>>()
            {
                { errorCode, [errorMessage] }
            }
        };
        
        return new HttpResult<TResponse>(errorDetails);
    }
}