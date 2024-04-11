using System.ComponentModel.DataAnnotations;

namespace ASSISTENTE.Contract.Internal.Knowledge.Queries.Answer;

public sealed class AnswerRequest
{
    [Required]
    [StringLength(8, ErrorMessage = "Name length can't be more than 8.")]
    public string Question { get; set; } = null!;
};