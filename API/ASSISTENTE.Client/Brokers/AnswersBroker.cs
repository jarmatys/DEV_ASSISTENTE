using ASSISTENTE.Client.Common.Models;
using ASSISTENTE.Contract.Internal.Requests.Knowledge.Queries.GenerateAnswer;

namespace ASSISTENTE.Client.Brokers;

public interface IAnswersBroker
{
    Task<HttpResult<GenerateAnswerResponse>> GenerateAnswerAsync(GenerateAnswerRequest request);
}

public sealed class AnswersBroker(HttpClient httpClient) : BrokerBase(httpClient, "api/answer"), IAnswersBroker
{
    public async Task<HttpResult<GenerateAnswerResponse>> GenerateAnswerAsync(GenerateAnswerRequest request)
    {
        return await PostAsync<GenerateAnswerResponse, GenerateAnswerRequest>(request);
    }
}