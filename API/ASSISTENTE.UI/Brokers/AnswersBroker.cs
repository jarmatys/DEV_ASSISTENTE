using ASSISTENTE.UI.Common.Models;
using ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GenerateAnswer;

namespace ASSISTENTE.UI.Brokers;

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