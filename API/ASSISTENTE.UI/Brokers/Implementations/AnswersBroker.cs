using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetAnswer;
using ASSISTENTE.UI.Common.Models;

namespace ASSISTENTE.UI.Brokers.Implementations;

public interface IAnswersBroker
{
    Task<HttpResult<GetAnswerResponse>> GetAnswerAsync(GetAnswerRequest request);
}

public sealed class AnswersBroker(HttpClient httpClient) : BrokerBase(httpClient, "api/answers"), IAnswersBroker
{
    public async Task<HttpResult<GetAnswerResponse>> GetAnswerAsync(GetAnswerRequest request)
    {
        return await GetAsync<GetAnswerResponse, GetAnswerRequest>(request);
    }
}