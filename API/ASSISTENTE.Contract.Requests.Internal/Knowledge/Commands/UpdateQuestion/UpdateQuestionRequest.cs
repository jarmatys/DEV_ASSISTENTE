using System.ComponentModel.DataAnnotations;

namespace ASSISTENTE.Contract.Requests.Internal.Knowledge.Commands.UpdateQuestion;

public sealed class UpdateQuestionRequest : RequestBase
{
    [Required]
    public string ConnectionId { get; set; } = null!;

    // TODO: Add state enum to inform client about current step
    
    public override void Clear()
    {
        ConnectionId = string.Empty;
    }
};