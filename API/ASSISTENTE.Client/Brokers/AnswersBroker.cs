using System.Net.Http.Json;
using System.Text.Json;
using ASSISTENTE.Client.Common.Exceptions;
using ASSISTENTE.Client.Common.Models;
using ASSISTENTE.Contract.Internal.Knowledge.Queries.Answer;

namespace ASSISTENTE.Client.Brokers;

public interface IAnswersBroker
{
    Task<HttpResult<AnswerResponse>> GenerateAnswerAsync(AnswerRequest request);
}

public sealed class AnswersBroker(HttpClient httpClient) : IAnswersBroker
{
    private const string RelativeUrl = "api/answer";
    
    public async Task<HttpResult<AnswerResponse>> GenerateAnswerAsync(AnswerRequest request)
    {
        var response = await httpClient.PostAsJsonAsync(RelativeUrl, request);

        if (response.IsSuccessStatusCode)
        {
            var successResult = await response.Content.ReadFromJsonAsync<AnswerResponse>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (successResult != null)
            {
                return HttpResult<AnswerResponse>.Success(successResult);
            }
        }
        
        var errorResult = await response.Content.ReadAsStringAsync();

        var errorResponse = JsonSerializer
            .Deserialize<ErrorResponse>(errorResult, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (errorResponse != null)
        {
            return HttpResult<AnswerResponse>.Failure(errorResponse);
        }
        
        throw new BrokerException("An error occurred while processing the request.");
    }
}