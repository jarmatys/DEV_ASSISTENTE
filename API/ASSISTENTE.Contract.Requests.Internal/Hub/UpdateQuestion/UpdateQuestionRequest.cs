using System.ComponentModel.DataAnnotations;
using ASSISTENTE.Contract.Requests.Internal.Common.RequestBases;

namespace ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestion;

public sealed class UpdateQuestionRequest : PutRequestBase
{
    [Required] public string ConnectionId { get; set; } = null!;

    [Required] public QuestionProgress Progress { get; set; }

    public override void Clear()
    {
        ConnectionId = string.Empty;
    }

    public static UpdateQuestionRequest Create(string connectionId, QuestionProgress progress)
    {
        return new UpdateQuestionRequest
        {
            ConnectionId = connectionId,
            Progress = progress
        };
    }
};