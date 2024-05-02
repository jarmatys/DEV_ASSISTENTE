using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestions.Models;

namespace ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestions;

public sealed record GetQuestionsResponse(List<QuestionDto> Questions);