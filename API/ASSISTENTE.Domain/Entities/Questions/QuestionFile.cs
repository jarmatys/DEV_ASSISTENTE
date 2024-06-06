using ASSISTENTE.Domain.Common;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.Questions;

public sealed class QuestionFile : AuditableEntity<QuestionFileId>
{
    private QuestionFile()
    {
    }
    
    private QuestionFile(string text)
    {
        Text = text;
    }

    public string Text { get; private set; } = null!;

    # region NAVIGATION PROPERTIES
    
    public QuestionId QuestionId { get; private set; } = null!;
    public Question Question { get; private set; } = null!;

    # endregion
    
    internal static Result<QuestionFile> Create(string text)
    {
        return new QuestionFile(text);
    }
}