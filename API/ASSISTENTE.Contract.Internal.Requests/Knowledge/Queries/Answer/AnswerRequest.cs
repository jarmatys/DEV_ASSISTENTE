using System.ComponentModel.DataAnnotations;

namespace ASSISTENTE.Contract.Internal.Requests.Knowledge.Queries.Answer;

public sealed class AnswerRequest : RequestBase
{
    [Required]
    [StringLength(300, ErrorMessage = "Name length can't be more than 300.")]
    public string Question { get; set; } = null!;

    public override void Clear()
    {
        Question = string.Empty;
    }
};