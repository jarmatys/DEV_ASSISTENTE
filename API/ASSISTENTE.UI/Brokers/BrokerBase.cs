using System.Net.Http.Json;
using System.Text.Json;
using ASSISTENTE.Contract.Requests.Internal.Common.RequestBases;
using ASSISTENTE.UI.Common.Models;

namespace ASSISTENTE.UI.Brokers;

public abstract class BrokerBase(HttpClient httpClient, string relativeUrl)
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected async Task<HttpResult<TResponse>> GetAsync<TResponse, TRequest>(TRequest request)
        where TRequest : GetRequestBase
    {
        try
        {
            var response = await httpClient.GetAsync($"{relativeUrl}?{request.QueryString()}");

            if (response.IsSuccessStatusCode)
            {
                var successResult = await response.Content.ReadFromJsonAsync<TResponse>(_jsonSerializerOptions);
                if (successResult != null)
                {
                    return HttpResult<TResponse>.Success(successResult);
                }
            }

            var errorResult = await response.Content.ReadAsStringAsync();

            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorResult, _jsonSerializerOptions);

            if (errorResponse != null)
            {
                return HttpResult<TResponse>.Failure(errorResponse);
            }
        }
        catch
        {
            return HttpResult<TResponse>.Failure(503, "InternalError", "API not available.");
        }

        return HttpResult<TResponse>.Failure(502, "BadGateway", "Wrong response from API.");
    }

    protected async Task<HttpResult> PostAsync<TRequest>(TRequest request)
        where TRequest : PostRequestBase
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(relativeUrl, request);

            if (response.IsSuccessStatusCode)
            {
                return HttpResult.Success();
            }

            var errorResult = await response.Content.ReadAsStringAsync();

            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorResult, _jsonSerializerOptions);

            if (errorResponse != null)
            {
                return HttpResult.Failure(errorResponse);
            }
        }
        catch
        {
            return HttpResult.Failure(503, "InternalError", "API not available.");
        }

        return HttpResult.Failure(502, "BadGateway", "Wrong response from API.");
    }
}