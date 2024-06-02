using System.ComponentModel.DataAnnotations;
using ASSISTENTE.Contract.Requests.Internal.Common.RequestBases;

namespace ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestionFailed;

public sealed class NotifyQuestionFailureRequest : PutRequestBase
{
    [Required] public required string ConnectionId { get; set; }

    public override void Clear()
    {
        ConnectionId = string.Empty;
    }

    public static NotifyQuestionFailureRequest Create(string connectionId)
    {
        return new NotifyQuestionFailureRequest
        {
            ConnectionId = connectionId,
        };
    }
};