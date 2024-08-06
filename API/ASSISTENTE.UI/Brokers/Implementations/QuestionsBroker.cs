using ASSISTENTE.Contract.Requests.Internal.Questions.Commands.CreateQuestion;
using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestion;
using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestions;
using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestionsCount;
using ASSISTENTE.Language.Identifiers;
using ASSISTENTE.UI.Brokers.Models;

namespace ASSISTENTE.UI.Brokers.Implementations;

public interface IQuestionsBroker
{
    Task<HttpResult> CreateQuestionAsync(CreateQuestionRequest request);
    Task<HttpResult<GetQuestionsResponse>> GetQuestionsAsync(GetQuestionsRequest request);
    Task<HttpResult<GetQuestionResponse>> GetQuestionAsync(GetQuestionRequest request);
    Task<HttpResult<GetQuestionsCountResponse>> GetQuestionsCountAsync();
}

public sealed class QuestionsBroker(IHttpClientFactory httpFactory, ILogger<QuestionsBroker> logger) 
    : BrokerBase(httpFactory, logger, "api/questions", BrokerConst.InternalApi), IQuestionsBroker
{
    public async Task<HttpResult> CreateQuestionAsync(CreateQuestionRequest request)
        => await PostAsync(request);

    public async Task<HttpResult<GetQuestionsResponse>> GetQuestionsAsync(GetQuestionsRequest request)
        => await GetAsync<GetQuestionsResponse, GetQuestionsRequest>(request);

    public async Task<HttpResult<GetQuestionResponse>> GetQuestionAsync(GetQuestionRequest request)
        => await GetDetailsAsync<GetQuestionResponse, QuestionId>(request.QuestionId);

    public async Task<HttpResult<GetQuestionsCountResponse>> GetQuestionsCountAsync()
        => await GetAsync<GetQuestionsCountResponse>("count");
}