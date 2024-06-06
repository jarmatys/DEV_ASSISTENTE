using ASSISTENTE.Domain.Common;
using ASSISTENTE.Domain.Entities.QuestionNotes.Events;
using ASSISTENTE.Domain.Entities.Questions;
using ASSISTENTE.Domain.Entities.Questions.Errors;
using ASSISTENTE.Language.Identifiers;

namespace ASSISTENTE.Domain.Entities.QuestionNotes;

public sealed class QuestionNote : AuditableEntity<QuestionNoteId>
{
    private QuestionNote()
    {
    }

    private QuestionNote(QuestionId questionId)
    {
        Embeddings = null;
        
        RaiseEvent(new QuestionNoteCreatedEvent(questionId));
    }
    
    public List<float>? Embeddings { get; private set; }

    # region NAVIGATION PROPERTIES

    public QuestionId QuestionId { get; private set; } = null!;
    public Question Question { get; private set; } = null!;

    # endregion

    internal static Result<QuestionNote> Create(QuestionId questionId)
    {
        return new QuestionNote(questionId);
    }
    
    internal Result AddEmbeddings(IEnumerable<float> embeddings)
    {
        Embeddings = embeddings.ToList();
        
        return Result.Success();
    }
    
    public Result<List<float>> GetEmbeddings()
    {
        return Embeddings ?? Result.Failure<List<float>>(QuestionErrors.EmbeddingsNotCreated.Build());
    }
}