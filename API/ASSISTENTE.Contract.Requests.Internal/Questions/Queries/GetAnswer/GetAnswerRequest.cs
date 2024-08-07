using System.ComponentModel.DataAnnotations;
using ASSISTENTE.Language.Identifiers;
using SOFTURE.Contract.Common.RequestBases;

namespace ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetAnswer;

public sealed class GetAnswerRequest : GetRequestBase
{
    [Required] public required QuestionId QuestionId { get; set; }
    
    public static GetAnswerRequest Create(QuestionId questionId)
    {
        return new GetAnswerRequest
        {
            QuestionId = questionId
        };
    }
}