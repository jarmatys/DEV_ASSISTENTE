using System.ComponentModel.DataAnnotations;

namespace ASSISTENTE.Contract.Internal.Requests.Knowledge.Commands.UpdateAnswer;

public sealed class UpdateAnswerRequest : RequestBase
{
    [Required]
    public string ConnectionId { get; set; } = null!;

    // TODO: Add state enum to inform client about current step
    
    public override void Clear()
    {
        ConnectionId = string.Empty;
    }
};