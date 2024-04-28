using System.ComponentModel.DataAnnotations;
using ASSISTENTE.Contract.Requests.Internal.Common.RequestBases;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Contract.Requests.Internal.Questions.Queries.GetAnswer;

public sealed class GetAnswerRequest : GetRequestBase
{
    [Required] public QuestionId QuestionId { get; set; } = null!;
}