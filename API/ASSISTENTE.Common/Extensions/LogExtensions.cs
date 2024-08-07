using ASSISTENTE.Common.Results;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.Common.Extensions;

public static class LogExtensions
{
    public static Result<T> LogError<T>(this Result<T> result, ILogger logger)
    {
        if (!result.IsFailure) return result;
        
        var error = Error.Parse(result.Error);
            
        logger.LogError("{ErrorType} - {ErrorDescription}", error.Type, error.Description);
        
        return result;
    }

    public static Result LogError(this Result result, ILogger logger)
    {
        if (!result.IsFailure) return result;
        
        var error = Error.Parse(result.Error);
            
        logger.LogError("{ErrorType} - {ErrorDescription}", error.Type, error.Description);

        return result;
    }

    public static Result<T> Log<T>(this Result<T> result, Func<T, string> successMessage, ILogger logger)
    {
        if (result.IsSuccess)
        {
            logger.LogInformation("Success: {SuccessMessage}", successMessage(result.Value));
        }

        return result;
    }

    public static Result Log(this Result result, string successMessage, ILogger logger)
    {
        if (result.IsSuccess)
        {
            logger.LogInformation("Success: {SuccessMessage}", successMessage);
        }

        return result;
    }
}