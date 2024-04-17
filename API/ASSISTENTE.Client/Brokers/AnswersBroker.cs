using ASSISTENTE.Client.Common.Models;
using ASSISTENTE.Contract.Internal.Requests.Knowledge.Queries.Answer;

namespace ASSISTENTE.Client.Brokers;

public interface IAnswersBroker
{
    Task<HttpResult<AnswerResponse>> GenerateAnswerAsync(AnswerRequest request);
}

public sealed class AnswersBroker(HttpClient httpClient) : BrokerBase(httpClient, "api/answer"), IAnswersBroker
{
    public async Task<HttpResult<AnswerResponse>> GenerateAnswerAsync(AnswerRequest request)
    {
        return await PostAsync<AnswerResponse, AnswerRequest>(request);
    }
}