using System.ComponentModel.DataAnnotations;
using ASSISTENTE.Contract.Requests.Internal.Common.RequestBases;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetAnswer;

public sealed class GetAnswerRequest : GetRequestBase
{
    [Required] public required Guid QuestionId { get; set; } // TODO: Implement endpoint binder to use strongy typed identifiers
    
    public static GetAnswerRequest Create(QuestionId questionId)
    {
        return new GetAnswerRequest
        {
            QuestionId = questionId
        };
    }
}