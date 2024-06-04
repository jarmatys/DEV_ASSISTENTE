using System.ComponentModel.DataAnnotations;
using ASSISTENTE.Contract.Requests.Internal.Common.RequestBases;

namespace ASSISTENTE.Contract.Requests.Internal.Hub.UpdateQuestionProgress;

public sealed class UpdateQuestionProgressRequest : PutRequestBase
{
    [Required] public required string ConnectionId { get; set; }
    [Required] public required int Progress { get; set; }
    [Required] public required string Information { get; set; }

    public override void Clear()
    {
        ConnectionId = string.Empty;
        Progress = default;
        Information = string.Empty;
    }

    public static UpdateQuestionProgressRequest Create(string connectionId, int progress, string information)
    {
        return new UpdateQuestionProgressRequest
        {
            ConnectionId = connectionId,
            Progress = progress,
            Information = information
        };
    }
};