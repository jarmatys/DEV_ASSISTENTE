using ASSISTENTE.Contract.Requests.Internal.Questions.Commands.CreateQuestion;
using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestions;
using ASSISTENTE.UI.Common.Models;

namespace ASSISTENTE.UI.Brokers.Implementations;

public interface IQuestionsBroker
{
    Task<HttpResult> CreateQuestionAsync(CreateQuestionRequest request);
    Task<HttpResult<GetQuestionsResponse>> GetQuestionsAsync(GetQuestionsRequest request);
}

public sealed class QuestionsBroker(HttpClient httpClient) : BrokerBase(httpClient, "api/questions"), IQuestionsBroker
{
    public async Task<HttpResult> CreateQuestionAsync(CreateQuestionRequest request)
        => await PostAsync(request);

    public async Task<HttpResult<GetQuestionsResponse>> GetQuestionsAsync(GetQuestionsRequest request)
        => await GetAsync<GetQuestionsResponse, GetQuestionsRequest>(request);
}