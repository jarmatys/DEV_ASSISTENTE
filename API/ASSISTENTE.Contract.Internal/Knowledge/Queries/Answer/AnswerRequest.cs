using System.ComponentModel.DataAnnotations;

namespace ASSISTENTE.Contract.Internal.Knowledge.Queries.Answer;

public sealed class AnswerRequest
{
    [Required]
    [StringLength(300, ErrorMessage = "Name length can't be more than 300.")]
    public string Question { get; set; } = null!;
};