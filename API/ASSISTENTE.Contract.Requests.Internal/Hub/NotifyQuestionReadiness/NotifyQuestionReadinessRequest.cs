using System.ComponentModel.DataAnnotations;
using ASSISTENTE.Language.Identifiers;
using SOFTURE.Contract.Common.RequestBases;

namespace ASSISTENTE.Contract.Requests.Internal.Hub.NotifyQuestionReadiness;

public sealed class NotifyQuestionReadinessRequest : PutRequestBase
{
    [Required] public required string ConnectionId { get; set; }
    [Required] public required QuestionId QuestionId { get; set; }

    public override void Clear()
    {
        ConnectionId = string.Empty;
    }

    public static NotifyQuestionReadinessRequest Create(string connectionId, QuestionId questionId)
    {
        return new NotifyQuestionReadinessRequest
        {
            ConnectionId = connectionId,
            QuestionId = questionId
        };
    }
};