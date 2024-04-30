using System.ComponentModel.DataAnnotations;
using ASSISTENTE.Contract.Requests.Internal.Common.RequestBases;
using ASSISTENTE.Language.Enums;

namespace ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestionProgress;

public sealed class UpdateQuestionProgressRequest : PutRequestBase
{
    [Required] public required string ConnectionId { get; set; }

    [Required] public required QuestionProgress Progress { get; set; }

    public override void Clear()
    {
        ConnectionId = string.Empty;
    }

    public static UpdateQuestionProgressRequest Create(string connectionId, QuestionProgress progress)
    {
        return new UpdateQuestionProgressRequest
        {
            ConnectionId = connectionId,
            Progress = progress
        };
    }
};