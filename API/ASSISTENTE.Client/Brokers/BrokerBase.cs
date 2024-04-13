using System.Net.Http.Json;
using System.Text.Json;
using ASSISTENTE.Client.Common.Exceptions;
using ASSISTENTE.Client.Common.Models;

namespace ASSISTENTE.Client.Brokers;

public abstract class BrokerBase(HttpClient httpClient, string relativeUrl)
{
    protected async Task<HttpResult<TResponse>> PostAsync<TResponse, TRequest>(TRequest request)
    {
        var response = await httpClient.PostAsJsonAsync(relativeUrl, request);

        if (response.IsSuccessStatusCode)
        {
            var successResult = await response.Content.ReadFromJsonAsync<TResponse>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (successResult != null)
            {
                return HttpResult<TResponse>.Success(successResult);
            }
        }
        
        var errorResult = await response.Content.ReadAsStringAsync();

        var errorResponse = JsonSerializer
            .Deserialize<ErrorResponse>(errorResult, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (errorResponse != null)
        {
            return HttpResult<TResponse>.Failure(errorResponse);
        }
        
        throw new BrokerException("An error occurred while processing the request.");
    }
}