using System.Net.Http.Json;
using System.Text.Json;
using ASSISTENTE.UI.Brokers.Models;
using Microsoft.Extensions.Logging;

namespace ASSISTENTE.UI.Brokers;

public abstract partial class BrokerBase
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    
    private async Task<HttpResult<TResponse>> HandleResponseAsync<TResponse>(HttpResponseMessage response)
        where TResponse : class
    {
        if (response.IsSuccessStatusCode)
        {
            var successResult = await response.Content.ReadFromJsonAsync<TResponse>(_jsonSerializerOptions);
            return successResult != default(TResponse)
                ? HttpResult<TResponse>.Success(successResult)
                : HttpResult<TResponse>.Failure(502, "BadGateway", "Wrong response from API.");
        }

        var errorResult = await response.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<ErrorResponse>(errorResult, _jsonSerializerOptions);
        
        return error == default(ErrorResponse) 
            ? HttpResult<TResponse>.Failure(502, "BadGateway", "Wrong response from API.") 
            : HttpResult<TResponse>.Failure(error);
    }

    private async Task<HttpResult> HandleResponseAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return HttpResult.Success();
        }

        var errorResult = await response.Content.ReadAsStringAsync();
        var error = JsonSerializer.Deserialize<ErrorResponse>(errorResult, _jsonSerializerOptions);

        return error == default(ErrorResponse) 
            ? HttpResult.Failure(502, "BadGateway", "Wrong response from API.") 
            : HttpResult.Failure(error);
    }
    
    private async Task<HttpResult<TResponse>> SendRequestAsync<TResponse>(Func<Task<HttpResponseMessage>> requestFunc)
        where TResponse : class
    {
        try
        {
            var response = await requestFunc();
            
            logger.LogInformation("UI request | {StatusCode}", response.StatusCode);
            
            return await HandleResponseAsync<TResponse>(response);
        }
        catch
        {
            return HttpResult<TResponse>.Failure(503, "InternalError", "API not available.");
        }
    }

    private async Task<HttpResult> SendRequestAsync(Func<Task<HttpResponseMessage>> requestFunc)
    {
        try
        {
            var response = await requestFunc();
            
            logger.LogInformation("UI request | {StatusCode}", response.StatusCode);

            return await HandleResponseAsync(response);
        }
        catch
        {
            return HttpResult.Failure(503, "InternalError", "API not available.");
        }
    }
}