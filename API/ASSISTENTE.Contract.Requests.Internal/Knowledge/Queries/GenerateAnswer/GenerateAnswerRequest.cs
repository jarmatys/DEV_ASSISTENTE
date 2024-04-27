using System.ComponentModel.DataAnnotations;

namespace ASSISTENTE.Contract.Requests.Internal.Knowledge.Queries.GenerateAnswer;

public sealed class GenerateAnswerRequest : RequestBase
{
    [Required]
    [StringLength(300, ErrorMessage = "Question length can't be more than 300.")]
    public string Question { get; set; } = null!;

    public override void Clear()
    {
        Question = string.Empty;
    }
};