using System.ComponentModel.DataAnnotations;

namespace ASSISTENTE.Contract.Requests.Internal.Knowledge.Commands.UpdateQuestion;

public sealed class UpdateQuestionRequest : RequestBase
{
    [Required]
    public string ConnectionId { get; set; } = null!;

    [Required]
    public QuestionProgress Progress { get; set; } 
    
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