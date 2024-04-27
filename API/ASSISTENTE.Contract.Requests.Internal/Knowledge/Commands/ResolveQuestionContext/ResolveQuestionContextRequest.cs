using System.ComponentModel.DataAnnotations;

namespace ASSISTENTE.Contract.Requests.Internal.Knowledge.Commands.ResolveQuestionContext;

public sealed class ResolveQuestionContextRequest : RequestBase
{
    [Required]
    [StringLength(300, ErrorMessage = "Question length can't be more than 300.")]
    public string Question { get; set; } = null!;
    
    [Required]
    public string ConnectionId { get; set; } = null!;

    public override void Clear()
    {
        Question = string.Empty;
        ConnectionId = string.Empty;
    }
};