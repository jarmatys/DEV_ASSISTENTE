using ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestion.Models;
using ASSISTENTE.Language.Enums;

namespace ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestion;

public sealed record GetQuestionResponse(
    string Text,
    string Answer,
    QuestionContext Context,
    List<ResourceDto> Resources
);