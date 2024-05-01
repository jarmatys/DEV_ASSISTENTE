using System.ComponentModel.DataAnnotations;
using ASSISTENTE.Contract.Requests.Internal.Common.RequestBases;

namespace ASSISTENTE.Contract.Requests.Internal.Questions.Commands.CreateQuestion;

public sealed class CreateQuestionRequest : PostRequestBase
{
    [Required]
    [StringLength(300, ErrorMessage = "Question length can't be more than 300.")]
    public string Question { get; set; } = null!;

    [Required] public string ConnectionId { get; set; } = null!;

    public override void Clear()
    {
        Question = string.Empty;
    }
};