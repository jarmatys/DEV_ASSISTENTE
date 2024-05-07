using System.ComponentModel.DataAnnotations;
using ASSISTENTE.Contract.Requests.Internal.Common.RequestBases;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetQuestion;

public sealed class GetQuestionRequest : GetRequestBase
{
    [Required] public required QuestionId QuestionId { get; set; }
    
    public static GetQuestionRequest Create(QuestionId questionId)
    {
        return new GetQuestionRequest
        {
            QuestionId = questionId
        };
    }
}